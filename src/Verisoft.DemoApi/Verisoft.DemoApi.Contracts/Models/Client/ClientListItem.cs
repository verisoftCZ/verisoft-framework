using Verisoft.DemoApi.Common.Enums;

namespace Verisoft.DemoApi.Contracts.Models.Client;

public class ClientListItem
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string TradeId { get; set; }

    public string VatId { get; set; }

    public Countries TaxDomicile { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public string UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
