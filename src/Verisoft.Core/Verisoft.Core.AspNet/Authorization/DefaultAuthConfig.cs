using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using Verisoft.Core.Common.Configuration;

namespace Verisoft.Core.AspNet.Authorization
{
    public class DefaultAuthConfig(IConfiguration configuration) : AutoBindConfig(configuration, "ApiAuthentication"), IAuthConfig
    {
        public IEnumerable<Application> Applications { get; set; } = Enumerable.Empty<Application>();
    }
}
