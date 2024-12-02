using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Verisoft.Core.Common.Configuration;

namespace Verisoft.Core.AspNet.ApplicationInsights
{
    public class AIConfig : AutoBindConfig
    {
        public AIConfig(IConfiguration configuration)
            : base(configuration, "ApplicationInsights")
        {
        }

        public Dictionary<string, string> DefaultProperties { get; set; } = new Dictionary<string, string>();
    }
}
