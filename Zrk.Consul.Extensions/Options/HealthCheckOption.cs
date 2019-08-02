using System;

namespace Zrk.Consul.Extensions.Options
{
    public class HealthCheckOption
    {
        /// <summary>
        /// 健康检查地址
        /// </summary>
        public string CheckUrl { get; set; } = "api/HealthCheck/Ping";
        /// <summary>
        /// 健康检查间隔
        /// </summary>
        public TimeSpan Interval { get; set; } = TimeSpan.FromSeconds(10);
    }
}
