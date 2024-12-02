using System;
using Microsoft.Extensions.Configuration;
using Verisoft.Core.Common.Configuration;

namespace Verisoft.Core.Data.EntityFramework.Configuration
{
    public class DatabaseConfig : IDatabaseConfig
    {
        public DatabaseConfig(IConfiguration configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            configuration.GetSection("Database").Bind(this);
        }

        public string ConnectionString { get; set; }

        public string DefaultSchema { get; set; }
    }
}
