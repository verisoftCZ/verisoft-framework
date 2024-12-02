using System;

namespace Verisoft.Core.Contracts.Audit;

public class EntityAuditFullDetail
{
    public Guid Id { get; set; }

    public Guid EntityAuditId { get; set; }

    public string Schema { get; set; }

    public string Table { get; set; }

    public string EntityId { get; set; }

    public string ChangedBy { get; set; }

    public DateTime ChangedAt { get; set; }

    public string ChangedAttribute { get; set; }

    public string OldValue { get; set; }

    public string NewValue { get; set; }
}
