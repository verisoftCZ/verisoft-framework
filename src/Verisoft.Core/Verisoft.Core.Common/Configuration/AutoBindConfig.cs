using Microsoft.Extensions.Configuration;

namespace Verisoft.Core.Common.Configuration
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