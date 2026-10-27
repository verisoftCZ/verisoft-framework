using Microsoft.Extensions.Configuration;
using System.Security;
using Verisoft.Core.Common.Configuration;

namespace Verisoft.Core.Common.Infrastructure
{
    public class ConnectionStringConfig : AutoBindConfig, IDatabaseConnectionString
    {
        public ConnectionStringConfig(IConfiguration configuration, string? section = null)
            : base(configuration, section ?? "Database")
        {
        }

        public string ConnectionString { get; set; } = string.Empty;
    }
}
