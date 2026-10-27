namespace Verisoft.Core.ApplicationInsights
{
    public interface ISimpleLogger
    {
        void TrackException(Exception ex);
    }
}
