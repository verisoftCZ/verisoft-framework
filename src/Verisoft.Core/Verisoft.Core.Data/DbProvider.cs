using Verisoft.Core.Common.Infrastructure;
using System;
using System.Data.SqlClient;

namespace Verisoft.Core.Data
{
    public class DbProvider
    {
        private readonly IDatabaseConnectionString connectionString;

        public DbProvider(IDatabaseConnectionString connectionString)
        {
            this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        protected SqlConnection CreateConnection() => new(connectionString.ConnectionString);
    }
}
