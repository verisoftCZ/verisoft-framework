using System;
using System.Collections.Generic;

namespace Verisoft.Core.Common.Entities;

public class EntityAuditEntity : Entity<Guid>
{
    public string Schema { get; set; }

    public string Table { get; set; }

    public string EntityId { get; set; }

    public string ChangedBy { get; set; }

    public DateTime ChangedAt { get; set; }

    public HashSet<EntityAuditDetailEntity> EntityAuditDetails { get; set; } = new HashSet<EntityAuditDetailEntity>();
}
