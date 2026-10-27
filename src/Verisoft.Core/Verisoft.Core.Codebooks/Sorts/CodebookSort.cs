using Verisoft.Core.Codebooks.Common.Entities;
using Verisoft.Core.Common.Entities;
using Verisoft.Core.Contracts;

namespace Verisoft.Core.Codebooks.Sorts;

public class CodebookSort : BaseSort<CodebookEntity>
{
    public override IReadOnlyList<SortClause<CodebookEntity>> DefaultSorts
        => [
            new(x => x.Name, SortDirection.Asc),
        ];
}
