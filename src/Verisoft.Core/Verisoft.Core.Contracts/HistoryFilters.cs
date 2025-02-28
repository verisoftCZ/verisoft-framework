using System;

namespace Verisoft.Core.Contracts;

public record HistoryFilters(
    HistoryQueryType QueryType,
    DateTime? StartUtc = null,
    DateTime? EndUtc = null);
