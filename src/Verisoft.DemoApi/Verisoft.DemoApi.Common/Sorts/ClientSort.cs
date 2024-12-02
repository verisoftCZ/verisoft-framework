using System.Linq.Expressions;
using Verisoft.Core.Common.Entities;
using Verisoft.DemoApi.Common.Entities;

namespace Verisoft.DemoApi.Common.Sorts;

public class ClientSort(string propertyName, bool isAscending, bool defaultAsc) : BaseSort<ClientEntity>(propertyName, isAscending, defaultAsc)
{
    public override Expression<Func<ClientEntity, object>> DefaultSort => x => x.Name;

    protected override Expression<Func<ClientEntity, object>> GetSortingExpression(string propertyName) => PropertyName switch
    {
        "id" => x => x.Id,
        "name" => x => x.Name,
        "representative" => x => x.Representative,
        "tradeId" => x => x.TradeId,
        "vatId" => x => x.VatId,
        "tradeRegisterEntry" => x => x.TradeRegisterEntry,
        "numberOfEmployees" => x => x.NumberOfEmployees,
        "taxDomicile" => x => x.TaxDomicile,
        "parentClientId" => x => x.ParentClientId,
        "createdby" => x => x.CreatedBy,
        "createdat" => x => x.CreatedAt,
        "updatedby" => x => x.UpdatedBy,
        "updatedat" => x => x.UpdatedAt,
        _ => DefaultSort,
    };
}
