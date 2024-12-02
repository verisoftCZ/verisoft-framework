using System;
using System.Linq;
using System.Linq.Expressions;
using Verisoft.Core.Common.Entities;
using Verisoft.Core.Common.Extenders;
using Verisoft.Core.Common.Linq;
using Verisoft.Core.Common.Linq.Specifications;

namespace Verisoft.Core.Common.Audit;

public class EntityAuditSpecification : ISpecification<EntityAuditEntity>
{
    public string EntityId { get; set; }

    public string ChangedBy { get; set; }

    public DateTime? ChangedAtFrom { get; set; }

    public DateTime? ChangedAtTo { get; set; }

    public string[] ChangedAttributes { get; set; } = [];

    public string OldValue { get; set; }

    public string NewValue { get; set; }

    public Expression<Func<EntityAuditEntity, bool>> SatisfiedBy()
    {
        Specification<EntityAuditEntity> specification = new TrueSpecification<EntityAuditEntity>();

        if (string.IsNullOrWhiteSpace(EntityId))
        {
            specification &= new DirectSpecification<EntityAuditEntity>(c =>
            c.EntityId.Contains(EntityId));
        }

        if (!string.IsNullOrWhiteSpace(ChangedBy))
        {
            specification &= new DirectSpecification<EntityAuditEntity>(c =>
            c.ChangedBy.Contains(ChangedBy));
        }

        if (ChangedAtFrom is not null)
        {
            specification &= new DirectSpecification<EntityAuditEntity>(c =>
            c.ChangedAt >= ChangedAtFrom.Value.ToUniversalTime());
        }

        if (ChangedAtTo is not null)
        {
            specification &= new DirectSpecification<EntityAuditEntity>(c =>
            c.ChangedAt <= ChangedAtTo.Value.ToUniversalTime());
        }

        if (ChangedAttributes.HasValue())
        {
            specification = specification.BuildOrSpecification(ChangedAttributes, searchValue => (entity) => entity.EntityAuditDetails.Any(detail => detail.ChangedAttribute.Contains(searchValue)));
        }

        if (!string.IsNullOrWhiteSpace(OldValue))
        {
            specification &= new DirectSpecification<EntityAuditEntity>(c =>
            c.EntityAuditDetails.Any(x => x.OldValue.Contains(OldValue)));
        }

        if (!string.IsNullOrWhiteSpace(NewValue))
        {
            specification &= new DirectSpecification<EntityAuditEntity>(c =>
            c.EntityAuditDetails.Any(x => x.NewValue.Contains(NewValue)));
        }

        return specification.SatisfiedBy();
    }
}
