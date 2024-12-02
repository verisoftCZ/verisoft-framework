using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Verisoft.Core.Common.Logging
{
    public interface IBusinessTracker
    {
        void TrackEvent(string name, IDictionary<string, string> values);

        void TrackMetric(string metricName, double value, IDictionary<string, string> properties = null);

        void TrackException(Exception ex, string customMessage = null);

        Task StartSelfAvailabilityTest(CancellationToken cancellationToken);

        void TrackSafely(Action<IBusinessTracker> action)
        {
            try
            {
                action(this);
            }
            catch (Exception ex)
            {
                this.TrackException(ex, "Error while tracking...");
            }
        }
    }
}
