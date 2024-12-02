using System;

namespace Verisoft.Core.Common.Logging
{
    public interface ISimpleLogger
    {
        void TrackException(Exception ex);
    }
}
