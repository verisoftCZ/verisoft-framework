namespace Verisoft.Core.AspNet.Hangfire
{
    public class HangfireConfig
    {
        public string HangfireSchemaName { get; set; } = null;

        public string[] CustomProcessingQueues { get; set; } = [];

        public string ConnectionString { get; set; } = string.Empty;

        public int NumberOfAutomaticRetries { get; set; } = 0;
    }
}
