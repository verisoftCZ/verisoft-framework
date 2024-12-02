namespace Verisoft.Core.MassTransit
{
    public class ApplicationInsightsConfig
    {
        public string ConnectionString { get; set; }

        public bool Enabled { get; set; } = true;
    }
}
