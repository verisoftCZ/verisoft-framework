using System;
using Microsoft.ApplicationInsights;
using Verisoft.Core.Common.Logging;

namespace Verisoft.Core.AspNet.ApplicationInsights
{
    public class AILogger : ISimpleLogger
    {
        private readonly TelemetryClient telemetryClient;

        public AILogger(TelemetryClient telemetryClient)
        {
            this.telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
        }

        public void TrackException(Exception ex)
            => telemetryClient.TrackException(ex);
    }
}
