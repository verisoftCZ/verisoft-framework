using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Verisoft.Core.ApplicationInsights
{
    public static class InstallExtensions
    {
        public static void AddVerisoftApplicationInsights(this IServiceCollection services, IConfiguration configuration, AIOptions options)
        {

            options = options ?? new AIOptions();

            services.AddApplicationInsightsTelemetry((ApplicationInsightsServiceOptions ai) =>
            {
                configuration.GetSection("ApplicationInsights").Bind(ai);
                ai.EnableDependencyTrackingTelemetryModule = options.EnableDependencyTrackingTelemetryModule;
                ai.EnablePerformanceCounterCollectionModule = options.EnablePerformanceCounterCollectionModule;
                ai.EnableQuickPulseMetricStream = options.EnablePerformanceCounterCollectionModule;
                ai.EnableEventCounterCollectionModule = options.EnablePerformanceCounterCollectionModule;
            });


            var setupActions = new (bool Enabled, Action Activate)[]
            {
            (!options.EnableHangfireTracking, () => services.AddApplicationInsightsTelemetryProcessor<NoSqlTelemetryProcessor>()),
            (options.EnableSqlCommandTextTracking,
                () => services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>((module, o) =>
                {
                    module.EnableSqlCommandTextInstrumentation = true;
                })),
            };

            foreach (var (enabled, activate) in setupActions)
            {
                if (enabled)
                {
                    activate.Invoke();
                }
            }

            services.AddSingleton<AIConfig>();
            services.AddTransient<IBusinessTracker, AITracker>();
        }

    }
}
