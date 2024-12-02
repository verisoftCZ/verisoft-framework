using System;
using System.Linq.Expressions;
using Verisoft.Core.Common.Entities;
using Verisoft.Core.Common.Extenders;
using Verisoft.Core.Common.Linq;
using Verisoft.Core.Common.Linq.Specifications;

namespace Verisoft.Core.Common.Audit;

public class EntityAuditDetailSpecification : ISpecification<EntityAuditDetailEntity>
{
    public string EntityId { get; set; }

    public string ChangedBy { get; set; }

    public DateTime? ChangedAtFrom { get; set; }

    public DateTime? ChangedAtTo { get; set; }

    public string[] ChangedAttributes { get; set; } = [];

    public string OldValue { get; set; }

    public string NewValue { get; set; }

    public Expression<Func<EntityAuditDetailEntity, bool>> SatisfiedBy()
    {
        Specification<EntityAuditDetailEntity> specification = new TrueSpecification<EntityAuditDetailEntity>();

        if (!string.IsNullOrWhiteSpace(EntityId))
        {
            specification &= new DirectSpecification<EntityAuditDetailEntity>(c =>
            c.EntityAudit.EntityId.Contains(EntityId));
        }

        if (!string.IsNullOrWhiteSpace(ChangedBy))
        {
            specification &= new DirectSpecification<EntityAuditDetailEntity>(c =>
            c.EntityAudit.ChangedBy.Contains(ChangedBy));
        }

        if (ChangedAtFrom is not null)
        {
            specification &= new DirectSpecification<EntityAuditDetailEntity>(c =>
            c.EntityAudit.ChangedAt >= ChangedAtFrom.Value.ToUniversalTime());
        }

        if (ChangedAtTo is not null)
        {
            specification &= new DirectSpecification<EntityAuditDetailEntity>(c =>
            c.EntityAudit.ChangedAt <= ChangedAtTo.Value.ToUniversalTime());
        }

        if (ChangedAttributes.HasValue())
        {
            specification = specification.BuildOrSpecification(ChangedAttributes, searchValue => (entity) => entity.ChangedAttribute.Contains(searchValue));
        }

        if (!string.IsNullOrWhiteSpace(OldValue))
        {
            specification &= new DirectSpecification<EntityAuditDetailEntity>(c =>
            c.OldValue.Contains(OldValue));
        }

        if (!string.IsNullOrWhiteSpace(NewValue))
        {
            specification &= new DirectSpecification<EntityAuditDetailEntity>(c =>
            c.NewValue.Contains(NewValue));
        }

        return specification.SatisfiedBy();
    }
}
