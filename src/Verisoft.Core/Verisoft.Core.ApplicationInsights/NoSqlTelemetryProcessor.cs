using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace Verisoft.Core.ApplicationInsights
{
    /// <summary>
    /// Data retention in appinsights is costly.
    /// Hangfire produces very large amount of nonsense data - every few seconds multiple lardge commands.
    /// This processor disables hangfire SQL dependencies, but allows tracking of other.
    /// </summary>
    public class NoSqlTelemetryProcessor : ITelemetryProcessor
    {
        private const string SQLDependencyName = "SQL";

        private List<string> ExcludedQueries = new List<string>()
        {
            "SELECT 1",
            "SELECT OBJECT_ID(N'[__IdentityMigrationsHistory]');",
            "SELECT OBJECT_ID(N'[__MigrationsHistory]');",
            "SELECT [MigrationId], [ProductVersion]\r\nFROM [__IdentityMigrationsHistory]\r\nORDER BY [MigrationId];",
            "SELECT [MigrationId], [ProductVersion]\r\nFROM [__MigrationsHistory]\r\nORDER BY [MigrationId];",
            "sp_getapplock",
            "sp_releaseapplock",
            "SELECT SYSUTCDATETIME()",
            "Commit"
        };

        public NoSqlTelemetryProcessor(ITelemetryProcessor next)
        {
            Next = next ?? throw new ArgumentNullException(nameof(next));
        }

        private ITelemetryProcessor Next { get; set; }

        public void Process(ITelemetry item)
        {
            if (IsSqlDependency(item))
            {
                return;
            }

            Next.Process(item);
        }

        private bool IsSqlDependency(ITelemetry item)
        {
            var dependency = item as DependencyTelemetry;
            if (dependency != null && dependency.Type == SQLDependencyName)
            {
                if(string.IsNullOrEmpty(dependency.Data))
                {
                    return true;
                }
                else if (ExcludedQueries.Contains(dependency.Data))
                {
                    return true;
                }
                else if (dependency.Data.ToLower().Contains("hangfire"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }
    }
}
