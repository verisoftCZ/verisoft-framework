using System.Collections.Generic;
using System.Linq;
using Verisoft.Core.Common.Entities;
using Verisoft.Core.Contracts.Audit;

namespace Verisoft.Core.Common.Audit;

public static class MappingExtensions
{
    public static EntityAuditSpecification ToSpecification(this EntityAuditFilter entityAuditFilter) => new()
    {
        EntityId = entityAuditFilter.EntityId,
        ChangedBy = entityAuditFilter.ChangedBy,
        ChangedAtFrom = entityAuditFilter.ChangedAtFrom,
        ChangedAtTo = entityAuditFilter.ChangedAtTo,
        ChangedAttributes = entityAuditFilter.ChangedAttributes,
        OldValue = entityAuditFilter.OldValue,
        NewValue = entityAuditFilter.NewValue,
    };

    public static EntityAuditDetailSpecification ToDetailSpecification(this EntityAuditFilter entityAuditFilter) => new()
    {
        EntityId = entityAuditFilter.EntityId,
        ChangedBy = entityAuditFilter.ChangedBy,
        ChangedAtFrom = entityAuditFilter.ChangedAtFrom,
        ChangedAtTo = entityAuditFilter.ChangedAtTo,
        ChangedAttributes = entityAuditFilter.ChangedAttributes,
        OldValue = entityAuditFilter.OldValue,
        NewValue = entityAuditFilter.NewValue,
    };

    public static EntityAudit ToContract(this EntityAuditEntity entityAuditEntity) => new()
    {
        Id = entityAuditEntity.Id,
        Schema = entityAuditEntity.Schema,
        Table = entityAuditEntity.Table,
        EntityId = entityAuditEntity.EntityId,
        ChangedBy = entityAuditEntity.ChangedBy,
        ChangedAt = entityAuditEntity.ChangedAt,
        EntityAuditDetails = [.. entityAuditEntity.EntityAuditDetails.Select(x => new EntityAuditDetail
            {
                Id = x.Id,
                ChangedAttribute = x.ChangedAttribute,
                OldValue = x.OldValue,
                NewValue = x.NewValue,
            })],
    };

    public static EntityAudit[] ToContract(this IEnumerable<EntityAuditEntity> entityAuditEntities) => [.. entityAuditEntities.Select(x => x.ToContract())];

    public static EntityAuditDetail ToContract(this EntityAuditDetailEntity entityAuditDetailEntity) => new()
    {
        Id = entityAuditDetailEntity.Id,
        ChangedAttribute = entityAuditDetailEntity.ChangedAttribute,
        OldValue = entityAuditDetailEntity.OldValue,
        NewValue = entityAuditDetailEntity.NewValue,
        EntityAudit = entityAuditDetailEntity.EntityAudit.ToContract(),
    };

    public static EntityAuditDetail[] ToContract(this IEnumerable<EntityAuditDetailEntity> entityAuditDetailEntities) => [.. entityAuditDetailEntities.Select(x => x.ToContract())];

    public static EntityAuditFullDetail ToFullDetailContract(this EntityAuditDetailEntity entityAuditDetailEntity) => new()
    {
        Id = entityAuditDetailEntity.Id,
        EntityAuditId = entityAuditDetailEntity.EntityAuditId,
        Schema = entityAuditDetailEntity.EntityAudit.Schema,
        Table = entityAuditDetailEntity.EntityAudit.Table,
        EntityId = entityAuditDetailEntity.EntityAudit.EntityId,
        ChangedBy = entityAuditDetailEntity.EntityAudit.ChangedBy,
        ChangedAt = entityAuditDetailEntity.EntityAudit.ChangedAt,
        ChangedAttribute = entityAuditDetailEntity.ChangedAttribute,
        OldValue = entityAuditDetailEntity.OldValue,
        NewValue = entityAuditDetailEntity.NewValue,
    };

    public static EntityAuditFullDetail[] ToFullDetailContract(this IEnumerable<EntityAuditDetailEntity> entityAuditDetailEntities) => [.. entityAuditDetailEntities.Select(x => x.ToFullDetailContract())];
}
