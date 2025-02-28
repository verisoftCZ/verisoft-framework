namespace Verisoft.CodebookApi.Contracts.Models.Base;

public class BasicTenantContract : ITenantValue
{
    public bool IsForbidden { get; set; }

    public int TenantId { get; set; }
}
