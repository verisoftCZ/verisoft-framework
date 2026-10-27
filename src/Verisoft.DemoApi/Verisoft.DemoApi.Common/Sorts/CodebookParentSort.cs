using Verisoft.Core.Common.Entities;
using Verisoft.Core.Contracts;
using Verisoft.DemoApi.Common.Entities;

namespace Verisoft.DemoApi.Common.Sorts;

public class CodebookParentSort : BaseSort<CodebookParentEntity>
{
    public override IReadOnlyList<SortClause<CodebookParentEntity>> DefaultSorts
        => [
            new(x => x.AccessibilityStringValue, SortDirection.Desc)
        ];
}
