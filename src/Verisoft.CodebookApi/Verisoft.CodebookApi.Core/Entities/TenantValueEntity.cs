using Verisoft.Core.Common.Entities;

namespace Verisoft.CodebookApi.Core.Entities;

public class TenantValueEntity : BaseEntity<int>
{
    public int TenantId { get; set; }

    public required string TableName { get; set; }

    public int CodebookRecordId { get; set; }

    public bool IsForbidden { get; set; }
}
