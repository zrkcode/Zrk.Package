using System;
using System.Net;
using Zrk.Consul.Extensions.Options;

namespace Zrk.Consul.ServiceExtensions.Options
{
    public class ServiceDiscoveryOption
    {
        public string ServiceName { get; set; }

        public HealthCheckOption HealthCheck { get; set; }

        public ConsulOption Consul { get; set; }

        public string[] Endpoints { get; set; }
    }

    public class ConsulOption
    {
        public string HttpEndpoint { get; set; }

        public DnsEndpoint DnsEndpoint { get; set; }
    }

    public class DnsEndpoint
    {
        public string Address { get; set; }

        public int Port { get; set; }

        public IPEndPoint ToIPEndPoint()
        {
            return new IPEndPoint(IPAddress.Parse(Address), Port);
        }
    }
}
