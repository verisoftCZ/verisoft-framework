using Microsoft.Extensions.Configuration;

namespace Verisoft.Core.ApplicationInsights
{
    public class AutoBindConfig
    {
        public AutoBindConfig(IConfiguration configuration, string configurationName)
        {
            if (configuration != null)
            {
                configuration.GetSection(configurationName).Bind(this);
            }
        }
    }
}
