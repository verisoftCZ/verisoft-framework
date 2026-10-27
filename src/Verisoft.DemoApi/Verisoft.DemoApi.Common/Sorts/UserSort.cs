using Verisoft.Core.Common.Entities;
using Verisoft.Core.Contracts;
using Verisoft.DemoApi.Common.Entities;

namespace Verisoft.DemoApi.Common.Sorts;

public sealed class UserSort : BaseSort<UserEntity>
{
    public override IReadOnlyList<SortClause<UserEntity>> DefaultSorts
        => [
            new(x => x.Surname, SortDirection.Asc)
        ];
}
