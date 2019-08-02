using Consul;
using DnsClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using Zrk.Consul.ServiceExtensions.Options;

namespace Zrk.Consul.ServiceExtensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomConsul(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<ServiceDiscoveryOption>(configuration.GetSection("ServiceDiscovery"));

            services.RegisterConsulClient();
            services.RegisterDnsLookup();
            return services;
        }

        public static IServiceCollection RegisterConsulClient(this IServiceCollection services)
        {
            services.TryAddSingleton<IConsulClient>(p => new ConsulClient(cfg =>
            {
                var serviceConfiguration = p.GetRequiredService<IOptions<ServiceDiscoveryOption>>().Value;

                if (!string.IsNullOrEmpty(serviceConfiguration?.Consul?.HttpEndpoint))
                {
                    // if not configured, the client will use the default value "127.0.0.1:8500"
                    cfg.Address = new Uri(serviceConfiguration.Consul.HttpEndpoint);
                }
            }));
            return services;
        }

        private static IServiceCollection RegisterDnsLookup(this IServiceCollection services)
        {
            //implement the dns lookup and register to service container
            services.TryAddSingleton<IDnsQuery>(p =>
            {
                var serviceConfiguration = p.GetRequiredService<IOptions<ServiceDiscoveryOption>>().Value;

                var client = new LookupClient(IPAddress.Parse("127.0.0.1"), 8600);
                if (serviceConfiguration.Consul.DnsEndpoint != null)
                {
                    client = new LookupClient(serviceConfiguration.Consul.DnsEndpoint.ToIPEndPoint());
                }
                return client;
            });
            return services;
        }
    }
}
