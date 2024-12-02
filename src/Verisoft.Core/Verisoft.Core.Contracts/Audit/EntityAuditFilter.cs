using System;

namespace Verisoft.Core.Contracts.Audit;

public class EntityAuditFilter
{
    public string EntityId { get; set; }

    public string ChangedBy { get; set; }

    public DateTime? ChangedAtFrom { get; set; }

    public DateTime? ChangedAtTo { get; set; }

    public string[] ChangedAttributes { get; set; } = [];

    public string OldValue { get; set; }

    public string NewValue { get; set; }
}
