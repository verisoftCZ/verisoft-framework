namespace Verisoft.Core.AspNet.ApplicationInsights
{
    public class AIOptions
    {
        public static AIOptions Default => new AIOptions
        {
            EnableHangfireTracking = false,
            EnableSqlCommandTextTracking = false,
        };

        public bool EnableSqlCommandTextTracking { get; set; } = false;

        /// <summary>
        /// You probably want this setting set to FALSE.
        /// Hangfire itself send too many same requests to DB.
        /// </summary>
        public bool EnableHangfireTracking { get; set; } = false;
    }
}
