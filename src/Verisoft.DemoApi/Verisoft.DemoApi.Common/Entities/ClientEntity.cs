using Verisoft.Core.Common.Entities;
using Verisoft.DemoApi.Common.Enums;

namespace Verisoft.DemoApi.Common.Entities;

public class ClientEntity : BaseEntity<int>, IPesimisticLockable
{
    public string Name { get; set; }

    public string Representative { get; set; }

    public string TradeId { get; set; }

    public string VatId { get; set; }

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

    public ClientEntity ParentClient { get; set; }

    public HashSet<ClientEntity> Subsidiaries { get; set; } = new();

    public ICollection<DocumentEntity> Documents { get; set; } = new List<DocumentEntity>();

    public string LockedBy { get; set; }

    public DateTime? LockedAt { get; set; }
}