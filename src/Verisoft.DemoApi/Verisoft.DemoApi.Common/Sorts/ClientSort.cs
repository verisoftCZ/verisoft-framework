using Verisoft.Core.Common.Entities;
using Verisoft.Core.Contracts;
using Verisoft.DemoApi.Common.Entities;

namespace Verisoft.DemoApi.Common.Sorts;

public class ClientSort : BaseSort<ClientEntity>
{
    public override IReadOnlyList<SortClause<ClientEntity>> DefaultSorts
        => [
            new(x => x.Name, SortDirection.Desc)
        ];
}
