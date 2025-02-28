using Verisoft.Core.Contracts;

namespace Verisoft.DemoApi.Contracts.Filters;

/// <summary>
/// Represents optional time-based filters for retrieving historical data.
/// </summary>
public class ClientHistoryFilter : ClientFilter
{
    /// <summary>
    /// The type of temporal query to perform (Current, AsOf, Between, or ContainedIn).
    /// </summary>
    public HistoryQueryType HistoryQueryType { get; init; }

    /// <summary>
    /// If <c>QueryType = AsOf</c>, pass the point in time. If <c>QueryType = Between</c> or <c>ContainedIn</c>, pass the start of the time range.
    /// </summary>
    public DateTime? PeriodStart { get; init; }

    /// <summary>
    /// If <c>QueryType = Between</c> or <c>ContainedIn</c>, pass the end of the time range.
    /// Not used if <c>QueryType = AsOf</c>.
    /// </summary>
    public DateTime? PeriodEnd { get; init; }
}