using System;
using System.Collections.Generic;

namespace Verisoft.Core.Contracts.Audit;

public class EntityAudit
{
    public Guid Id { get; set; }

    public string Schema { get; set; }

    public string Table { get; set; }

    public string EntityId { get; set; }

    public string ChangedBy { get; set; }

    public DateTime ChangedAt { get; set; }

    public HashSet<EntityAuditDetail> EntityAuditDetails { get; set; }
}
