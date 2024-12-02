using System;

namespace Verisoft.Core.Contracts.Audit;

public class EntityAuditDetail
{
    public Guid Id { get; set; }

    public string ChangedAttribute { get; set; }

    public string OldValue { get; set; }

    public string NewValue { get; set; }

    public EntityAudit EntityAudit { get; set; }
}
