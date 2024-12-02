using System.Linq.Expressions;
using Verisoft.Core.Common.Entities;
using Verisoft.DemoApi.Common.Entities;

namespace Verisoft.DemoApi.Common.Sorts;

public class DocumentSort(string propertyName, bool isAscending, bool defaultAsc) : BaseSort<DocumentEntity>(propertyName, isAscending, defaultAsc)
{
    public override Expression<Func<DocumentEntity, object>> DefaultSort => x => x.Name;

    protected override Expression<Func<DocumentEntity, object>> GetSortingExpression(string propertyName) => PropertyName switch
    {
        "id" => x => x.Id,
        "name" => x => x.Name,
        "contenttype" => x => x.ContentType,
        "clientid" => x => x.ClientId,
        "description" => x => x.Description,
        "createdby" => x => x.CreatedBy,
        "createdat" => x => x.CreatedAt,
        "updatedby" => x => x.UpdatedBy,
        "updatedat" => x => x.UpdatedAt,
        _ => DefaultSort,
    };
}