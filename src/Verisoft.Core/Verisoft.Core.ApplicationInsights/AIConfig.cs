using Microsoft.Extensions.Configuration;


namespace Verisoft.Core.ApplicationInsights
{
    public class AIConfig : AutoBindConfig
    {
        public AIConfig(IConfiguration configuration)
            : base(configuration, "ApplicationInsights")
        {
        }

        public int AvailabilityEveryMinutes { get; set; } = 5;
        public string AvailabilityTestName { get; set; } = "AvailabilityTest";
        public string RunLocation { get; set; } = "Czechia";

        public Dictionary<string, string> DefaultProperties { get; set; } = new Dictionary<string, string>();
    }
}
