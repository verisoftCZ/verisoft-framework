using Verisoft.Core.Common.Entities;
using Verisoft.Core.Common.Entities.Codebooks;
using Verisoft.Core.Contracts;

namespace Verisoft.Core.Codebooks.Sorts;

public class CodebookRecordSort<T> : BaseSort<CodebookRecordEntity<T>>
    where T : CodebookRecordType, new()
{
    public override IReadOnlyList<SortClause<CodebookRecordEntity<T>>> DefaultSorts
        => [
            new(x => x.StringValue, SortDirection.Asc),
        ];
}
