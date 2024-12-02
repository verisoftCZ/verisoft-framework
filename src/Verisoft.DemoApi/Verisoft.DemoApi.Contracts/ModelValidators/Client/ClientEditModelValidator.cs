using FluentValidation;
using Verisoft.DemoApi.Contracts.Models.Client;

namespace Verisoft.DemoApi.Contracts.ModelValidators;

public class ClientEditModelValidator : AbstractValidator<ClientEditModel>
{
    public ClientEditModelValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.TradeId).NotNull().Matches(@"^[0-9]{8}$");
        RuleFor(x => x.VatId).NotNull().Matches(@"^CZ[0-9]{8}$");
        RuleFor(x => x.Representative).MinimumLength(1);
        RuleFor(x => x.TradeRegisterEntry).MinimumLength(1);
        RuleFor(x => x.NumberOfEmployees).GreaterThan(0).LessThan(100000);
        RuleFor(x => x.Description).MinimumLength(1).MaximumLength(100);
        RuleFor(x => x.ContactNote).MinimumLength(1).MaximumLength(100);
        RuleFor(x => x.AccountingSystemClientId).MinimumLength(1);
        RuleFor(x => x.WebsiteUrl).Matches(ValidatorConsts.ValidUrlRegex);
        RuleFor(x => x.RecruitmentUrl).Matches(ValidatorConsts.ValidUrlRegex);
        RuleFor(x => x.CompanyActivity).MinimumLength(1).MaximumLength(100);
        RuleFor(x => x.TaxDomicile).IsInEnum();
        RuleFor(x => x.ParentClientId);
    }
}