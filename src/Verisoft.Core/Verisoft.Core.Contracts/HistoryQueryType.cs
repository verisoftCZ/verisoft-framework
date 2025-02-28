namespace Verisoft.Core.Contracts;

/// <summary>
/// Specifies the desired type of temporal query.
/// </summary>
public enum HistoryQueryType
{
    /// <summary>
    /// Retrieve data as it existed at a specific point in time.
    /// </summary>
    AsOf,

    /// <summary>
    /// Retrieve data that overlapped with a time range.
    /// </summary>
    Between,

    /// <summary>
    /// Retrieve data that was fully contained in a time range.
    /// </summary>
    ContainedIn,
}