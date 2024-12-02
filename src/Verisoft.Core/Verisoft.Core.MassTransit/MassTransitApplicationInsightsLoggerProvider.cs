using System;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.Extensions.Options;

namespace Verisoft.Core.MassTransit
{
    internal class MassTransitApplicationInsightsLoggerProvider : ILoggerProvider
    {
        private readonly IMassTransitConfig config;

        private ApplicationInsightsLoggerProvider applicationInsightsLoggerProvider;

        public MassTransitApplicationInsightsLoggerProvider(IMassTransitConfig config)
        {
            this.config = config;
            this.Initialize();
        }

        public ILogger CreateLogger(string categoryName)
        {
            if (this.applicationInsightsLoggerProvider == null)
            {
                throw new InvalidOperationException("The application insights provider is not initialized.");
            }

            return this.applicationInsightsLoggerProvider.CreateLogger(categoryName);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            this.applicationInsightsLoggerProvider?.Dispose();
        }

        private void Initialize()
        {
            TelemetryConfiguration configuration = TelemetryConfiguration.CreateDefault();
            configuration.ConnectionString = this.config.ApplicationInsights.ConnectionString;
            configuration.TelemetryInitializers.Add(new HttpDependenciesParsingTelemetryInitializer());

            var loggerOptions = new ApplicationInsightsLoggerOptions();

            this.applicationInsightsLoggerProvider = new ApplicationInsightsLoggerProvider(
                Options.Create(configuration),
                Options.Create(loggerOptions));

            ILoggerFactory factory = new LoggerFactory();
            factory.AddProvider(applicationInsightsLoggerProvider);
        }
    }
}
