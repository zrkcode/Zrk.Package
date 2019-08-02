using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using Zrk.Consul.Extensions.Options;
using Zrk.Consul.ServiceExtensions.Options;

namespace Zrk.Consul.ServiceExtensions
{
    public static class BuilderExtensions
    {
        public static IApplicationBuilder UseCustomConsul(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            var lifetime = app.ApplicationServices.GetService(typeof(IApplicationLifetime)) as IApplicationLifetime;

            var serviceDiscoveryOption = app.ApplicationServices.GetService(typeof(IOptions<ServiceDiscoveryOption>)) as IOptions<ServiceDiscoveryOption>;
            var consul = app.ApplicationServices.GetService(typeof(IConsulClient)) as IConsulClient;

            app.UseConsulRegisterService(serviceDiscoveryOption.Value, consul, lifetime);

            return app;
        }

        private static IApplicationBuilder UseConsulRegisterService(this IApplicationBuilder app, ServiceDiscoveryOption serviceDiscoveryOption, IConsulClient consul, IApplicationLifetime lifetime)
        {
            var loggerFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("ServiceBuilder");

            IEnumerable<Uri> addresses = null;
            if (serviceDiscoveryOption.Endpoints != null && serviceDiscoveryOption.Endpoints.Length > 0)
            {
                logger.LogInformation($"Using {serviceDiscoveryOption.Endpoints.Length} configured endpoints for service registration");
                addresses = serviceDiscoveryOption.Endpoints.Select(p => new Uri(p));
            }
            else
            {
                logger.LogInformation($"Trying to use server.Features to figure out the service endpoint for registration.");
                var features = app.Properties["server.Features"] as FeatureCollection;
                addresses = features.Get<IServerAddressesFeature>().Addresses.Select(p => new Uri(p)).ToArray();
            }

            foreach (var address in addresses)
            {
                var serviceId = $"{serviceDiscoveryOption.ServiceName}_{address.Host}:{address.Port}";

                var httpCheck = new AgentServiceCheck()
                {
                    Interval = serviceDiscoveryOption.HealthCheck.Interval,
                    HTTP = new Uri(address, serviceDiscoveryOption.HealthCheck.CheckUrl).OriginalString
                };

                var registration = new AgentServiceRegistration()
                {
                    Checks = new[] { httpCheck },
                    Address = address.Host,
                    ID = serviceId,
                    Name = serviceDiscoveryOption.ServiceName,
                    Port = address.Port
                };

                consul.Agent.ServiceRegister(registration).GetAwaiter().GetResult();

                // register service & health check cleanup
                lifetime.ApplicationStopping.Register(() =>
                {
                    logger.LogInformation("Removing tenant & additional health check");
                    consul.Agent.ServiceDeregister(serviceId).GetAwaiter().GetResult();
                });
            }

            return app;
        }
    }
}
