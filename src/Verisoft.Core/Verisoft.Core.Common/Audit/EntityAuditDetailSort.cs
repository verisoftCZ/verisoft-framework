using System;
using System.Linq.Expressions;
using Verisoft.Core.Common.Entities;

namespace Verisoft.Core.Common.Audit;

public class EntityAuditDetailSort(string propertyName, bool isAscending, bool defaultAsc)
    : BaseSort<EntityAuditDetailEntity>(propertyName, isAscending, defaultAsc)
{
    public override Expression<Func<EntityAuditDetailEntity, object>> DefaultSort
    {
        get
        {
            return x => x.EntityAudit.ChangedAt;
        }
    }

    protected override Expression<Func<EntityAuditDetailEntity, object>> GetSortingExpression(string propertyName) =>
    propertyName switch
    {
        "newvalue" => x => x.NewValue,
        "oldvalue" => x => x.OldValue,
        "changedattribute" => x => x.ChangedAttribute,
        "changedby" => x => x.EntityAudit.ChangedBy,
        "changedat" => x => x.EntityAudit.ChangedAt,
        "schema" => x => x.EntityAudit.Schema,
        "table" => x => x.EntityAudit.Table,
        "entityid" => x => x.EntityAudit.EntityId,
        _ => DefaultSort,
    };
}
