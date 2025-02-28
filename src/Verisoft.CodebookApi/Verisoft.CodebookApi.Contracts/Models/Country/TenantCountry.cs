using Verisoft.CodebookApi.Contracts.Models.Base;

namespace Verisoft.CodebookApi.Contracts.Models.Country;

public class TenantCountry : Country, ITenantValue
{
    public bool IsForbidden { get; set; }

    public int TenantId { get; set; }
}
