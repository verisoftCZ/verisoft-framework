using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System;

namespace Verisoft.Core.AspNet.Hangfire
{
    /// <summary>
    /// Data retention in appinsights is costly.
    /// Hangfire produces very large amount of nonsense data - every few seconds multiple lardge commands.
    /// This processor disables hangfire SQL dependencies, but allows tracking of other.
    /// </summary>
    public class NoSqlTelemetryProcessor : ITelemetryProcessor
    {
        private const string SQLDependencyName = "SQL";

        public NoSqlTelemetryProcessor(ITelemetryProcessor next)
        {
            this.Next = next ?? throw new ArgumentNullException(nameof(next));
        }

        private ITelemetryProcessor Next { get; set; }

        public void Process(ITelemetry item)
        {
            if (IsSqlDependency(item))
            {
                return;
            }

            this.Next.Process(item);
        }

        private bool IsSqlDependency(ITelemetry item)
        {
            var dependency = item as DependencyTelemetry;
            if (dependency != null && dependency.Type == SQLDependencyName)
            {
                return true;
            }

            return false;
        }
    }
}
