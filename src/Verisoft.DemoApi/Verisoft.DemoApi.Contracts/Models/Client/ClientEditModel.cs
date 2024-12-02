using Verisoft.DemoApi.Common.Enums;

namespace Verisoft.DemoApi.Contracts.Models.Client;

public class ClientEditModel
{
    public string Name { get; set; }

    public string TradeId { get; set; }

    public string VatId { get; set; }

    public string Representative { get; set; }

    public string TradeRegisterEntry { get; set; }

    public int? NumberOfEmployees { get; set; }

    public string Description { get; set; }

    public string ContactNote { get; set; }

    public string AccountingSystemClientId { get; set; }

    public string WebsiteUrl { get; set; }

    public string RecruitmentUrl { get; set; }

    public string CompanyActivity { get; set; }

    public Countries TaxDomicile { get; set; }

    public int? ParentClientId { get; set; }
}
