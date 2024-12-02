namespace Verisoft.Core.MassTransit
{
    public interface IMassTransitConfig
    {
        MassTransitServerConfig Server { get; }

        ApplicationInsightsConfig ApplicationInsights { get; }
    }
}
