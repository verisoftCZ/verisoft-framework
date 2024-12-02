using System;

namespace Verisoft.Core.Common.Infrastructure
{
    public class DatabaseConnectionString : IDatabaseConnectionString
    {
        private readonly string connectionString;

        public DatabaseConnectionString(string connectionString)
        {
            this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public string ConnectionString => connectionString;
    }
}
