using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Verisoft.Core.Common.Configuration;

namespace Verisoft.Core.AspNet.Authorization
{
    public class DefaultAuthConfig : AutoBindConfig, IAuthConfig
    {
        public DefaultAuthConfig(IConfiguration configuration)
            : base(configuration, "ApiAuthentication")
        {
        }

        public IEnumerable<Application> Applications { get; set; }
    }
}
