using System;
using System.Linq.Expressions;
using Verisoft.Core.Common.Entities;

namespace Verisoft.Core.Common.Audit;

public class EntityAuditSort(string propertyName, bool isAscending, bool defaultAsc)
    : BaseSort<EntityAuditEntity>(propertyName, isAscending, defaultAsc)
{
    public override Expression<Func<EntityAuditEntity, object>> DefaultSort
    {
        get
        {
            return x => x.ChangedAt;
        }
    }

    protected override Expression<Func<EntityAuditEntity, object>> GetSortingExpression(string propertyName) =>
    propertyName switch
    {
        "schema" => x => x.Schema,
        "table" => x => x.Table,
        "entityid" => x => x.EntityId,
        "changedby" => x => x.ChangedBy,
        "changedat" => x => x.ChangedAt,
        _ => DefaultSort,
    };
}
