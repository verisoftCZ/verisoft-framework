using System;

namespace Verisoft.Core.Common.Entities;

public class EntityAuditDetailEntity : Entity<Guid>
{
    public string ChangedAttribute { get; set; }

    public string OldValue { get; set; }

    public string NewValue { get; set; }

    public EntityAuditEntity EntityAudit { get; set; }

    public Guid EntityAuditId { get; set; }
}
