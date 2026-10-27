using System.Collections.Generic;

namespace Verisoft.Core.Contracts.Export;

public sealed record ExportDataRequest<TFilter>(
    IEnumerable<SortDefinition>? Sorts,
    TFilter? Filter,
    IReadOnlyCollection<ColumnToExport> ColumnsToExport);