using Verisoft.Core.Common.Entities;
using Verisoft.Core.Contracts;
using Verisoft.DemoApi.Common.Entities;

namespace Verisoft.DemoApi.Common.Sorts;

public class DocumentSort : BaseSort<DocumentEntity>
{
    public override IReadOnlyList<SortClause<DocumentEntity>> DefaultSorts
        => [
            new(x => x.Name, SortDirection.Asc)
        ];
}