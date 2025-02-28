namespace Verisoft.CodebookApi.Contracts.Models.Base;

public interface ITenantValue
{
    public bool IsForbidden { get; set; }

    public int TenantId { get; set; }
}
