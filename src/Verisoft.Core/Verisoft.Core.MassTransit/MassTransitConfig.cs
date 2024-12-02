using System;
using Microsoft.Extensions.Configuration;

namespace Verisoft.Core.MassTransit
{
    public class MassTransitConfig : IMassTransitConfig
    {
        private const string MassTransitConfigName = "MassTransit";

        private const string ApplicationInsightsConfigName = "ApplicationInsights";

        public MassTransitConfig(IConfiguration configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            configuration.GetSection(MassTransitConfigName).Bind(this);
            if (string.IsNullOrEmpty(this.ApplicationInsights?.ConnectionString))
            {
                var applicationInsightsConfig = configuration.GetSection(ApplicationInsightsConfigName);
                applicationInsightsConfig?.Bind(this.ApplicationInsights);
            }
        }

        public MassTransitServerConfig Server { get; set; } = new MassTransitServerConfig();

        public ApplicationInsightsConfig ApplicationInsights { get; set; } = new ApplicationInsightsConfig();
    }
}