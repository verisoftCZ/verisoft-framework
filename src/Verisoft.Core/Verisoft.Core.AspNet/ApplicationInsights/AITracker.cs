using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Verisoft.Core.Common.Logging;

namespace Verisoft.Core.AspNet.ApplicationInsights
{
    public class AITracker : IBusinessTracker
    {
        private const int AvailabilityEveryMinutes = 5;
        private const string AvailabilityTestName = "selftest";
        private const string RunLocation = "Czechia"; // Filip: parametrizovat?
        private readonly TelemetryClient telemetryClient;
        private readonly AIConfig config;

        public AITracker(TelemetryClient telemetryClient, AIConfig config)
        {
            this.telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
            this.config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public Task StartSelfAvailabilityTest(CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    try
                    {
                        if (cancellationToken.CanBeCanceled && cancellationToken.IsCancellationRequested)
                        {
                            return;
                        }

                        this.telemetryClient.TrackAvailability(AvailabilityTestName, DateTime.Now, TimeSpan.FromMilliseconds(100), RunLocation, true, null, GetProperties());
                        await Task.Delay((int)TimeSpan.FromMinutes(AvailabilityEveryMinutes).TotalMilliseconds);
                    }
                    catch (Exception ex)
                    {
                        this.telemetryClient.TrackException(ex);
                    }
                }
            });
        }

        public void TrackEvent(string name, IDictionary<string, string> properties)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
            }

            telemetryClient.TrackEvent(name, GetProperties(properties));
        }

        public void TrackException(Exception ex, string customMessage = null)
        {
            var properties = GetProperties();
            if (!string.IsNullOrEmpty(customMessage))
            {
                properties.Add("message", customMessage);
            }

            telemetryClient.TrackException(ex, properties);
        }

        public void TrackMetric(string metricName, double value, IDictionary<string, string> properties = null)
        {
            if (string.IsNullOrEmpty(metricName))
            {
                throw new ArgumentException($"'{nameof(metricName)}' cannot be null or empty.", nameof(metricName));
            }

            telemetryClient.TrackMetric(metricName, value, GetProperties(properties));
        }

        private IDictionary<string, string> GetProperties(IDictionary<string, string> source = null)
        {
            var defaults = new Dictionary<string, string>(this.config.DefaultProperties ?? new Dictionary<string, string>());
            if (source == null)
            {
                return defaults;
            }

            return MergeSafely(defaults, source);
        }

        private IDictionary<string, string> MergeSafely(Dictionary<string, string> defaults, IDictionary<string, string> source)
        {
            var result = source;
            foreach (var kv in defaults)
            {
                if (!result.ContainsKey(kv.Key))
                {
                    result.Add(kv.Key, kv.Value);
                }
            }

            return result;
        }
    }
}