using System.Linq.Expressions;
using Verisoft.Core.Common.Extenders;
using Verisoft.Core.Common.Linq;
using Verisoft.Core.Common.Linq.Specifications;
using Verisoft.DemoApi.Common.Entities;
using Verisoft.DemoApi.Common.Enums;

namespace Verisoft.DemoApi.Common.Specifications;

public class ClientSpecification : ISpecification<ClientEntity>
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

    public Expression<Func<ClientEntity, bool>> SatisfiedBy()
    {
        Specification<ClientEntity> specification = new TrueSpecification<ClientEntity>();
        if (SearchField.HasValue<string>())
        {
            var nameSpecification = new DirectSpecification<ClientEntity>(c => c.Name.Contains(SearchField));
            var tradeIdSpecification = new DirectSpecification<ClientEntity>(c => c.TradeId.Contains(SearchField));
            var representativeSpecification = new DirectSpecification<ClientEntity>(c => c.Representative.Contains(SearchField));
            specification = SpecificationExtender.OrAll(nameSpecification, tradeIdSpecification, representativeSpecification);
        }

        if (Name.HasValue())
        {
            specification = specification.BuildOrSpecification(Name, searchValue => (entity) => entity.Name.Contains(searchValue));
        }

        if (TradeId.HasValue())
        {
            specification = specification.BuildOrSpecification(TradeId, searchValue => (entity) => entity.TradeId.Contains(searchValue));
        }

        if (VatId.HasValue())
        {
            specification = specification.BuildOrSpecification(VatId, searchValue => (entity) => entity.VatId.Contains(searchValue));
        }

        if (Representative.HasValue())
        {
            specification = specification.BuildOrSpecification(Representative, searchValue => (entity) => entity.Representative.Contains(searchValue));
        }

        if (TradeRegisterEntry.HasValue())
        {
            specification = specification.BuildOrSpecification(TradeRegisterEntry, searchValue => (entity) => entity.TradeRegisterEntry.Contains(searchValue));
        }

        if (AccountingSystemClientId.HasValue())
        {
            specification = specification.BuildOrSpecification(AccountingSystemClientId, searchValue => (entity) => entity.AccountingSystemClientId.Contains(searchValue));
        }

        if (CompanyActivity.HasValue())
        {
            specification = specification.BuildOrSpecification(CompanyActivity, searchValue => (entity) => entity.CompanyActivity.Contains(searchValue));
        }

        if (TaxDomicile.HasValue())
        {
            specification = specification.BuildOrSpecification(TaxDomicile, searchValue => (entity) => entity.TaxDomicile == searchValue);
        }

        return specification.SatisfiedBy();
    }
}