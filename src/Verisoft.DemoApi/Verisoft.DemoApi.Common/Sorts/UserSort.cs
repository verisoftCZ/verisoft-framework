using System.Linq.Expressions;
using Verisoft.Core.Common.Entities;
using Verisoft.DemoApi.Common.Entities;

namespace Verisoft.DemoApi.Common.Sorts;

public sealed class UserSort(string propertyName, bool isAscending, bool defaultAsc) : BaseSort<UserEntity>(propertyName, isAscending, defaultAsc)
{
    public override Expression<Func<UserEntity, object>> DefaultSort
    {
        get { return x => x.Surname; }
    }

    protected override Expression<Func<UserEntity, object>> GetSortingExpression(string propertyName) => PropertyName switch
    {
        "id" => x => x.Id,
        "firstname" => x => x.FirstName,
        "surname" => x => x.Surname,
        "email" => x => x.Email,
        "documents" => x => x.Documents,
        _ => DefaultSort,
    };
}
