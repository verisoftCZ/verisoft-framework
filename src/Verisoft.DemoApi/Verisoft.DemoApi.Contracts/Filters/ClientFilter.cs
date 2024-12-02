using Verisoft.DemoApi.Common.Enums;

namespace Verisoft.DemoApi.Contracts.Filters;

public class ClientFilter
{
    public string[] Name { get; set; }

    public string[] TradeId { get; set; }

    public string[] VatId { get; set; }

    public string[] Representative { get; set; }

    public string[] TradeRegisterEntry { get; set; }

    public string[] AccountingSystemClientId { get; set; }

    public string[] CompanyActivity { get; set; }

    public Countries[] TaxDomicile { get; set; }

    public string SearchField { get; set; }
}
