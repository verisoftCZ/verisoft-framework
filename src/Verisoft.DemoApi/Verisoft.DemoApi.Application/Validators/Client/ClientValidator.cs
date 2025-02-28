using FluentValidation;
using Verisoft.Core.Validation.Validators;
using Verisoft.DemoApi.Common.Repositories;
using Verisoft.DemoApi.Contracts.Models.Client;

namespace Verisoft.DemoApi.Application.Validators.Client;

public class ClientValidator : AbstractValidator<ClientValidatedObject>
{
    private const string MustBeAValidTaxDomicileIsoCode = "Must be a valid tax domicile ISO Code";
    private const string ClientWithSameTradeIdAlreadyExists = "Client with same Trade ID already exists";
    private const string ClientWithSameVatNumberAlreadyExists = "Client with same VAT number already exists";
    private const string ParentClientWithThisIdDoesNotExist = "Parent Client with this Id does not exist";
    private const string WebsiteUrlIsNotValid = "Website Url is not valid";
    private const string RecruitmentUrlIsNotValid = "Recruitment Url is not valid";

    private const string NameNotEmpty = "NameNotEmpty";
    private const string NameMaxLength = "NameMaxLength";
    private const string TradeIdNotEmpty = "TradeIdNotEmpty";
    private const string TradeIdMaxLength = "TradeIdMaxLength";
    private const string TradeIdAlreadyExists = "TradeIdAlreadyExists";
    private const string VatIdAlreadyExists = "VatIdAlreadyExists";
    private const string VatIdMaxLength = "VatIdMaxLength";
    private const string VatIdInvalidFormat = "VatIdInvalidFormat";
    private const string RepresentativeNotEmpty = "RepresentativeNotEmpty";
    private const string RepresentativeMaxLength = "RepresentativeMaxLength";
    private const string EmployeesGreaterThanOrEqualToZero = "EmployeesGreaterThanOrEqualToZero";
    private const string WebsiteUrlInvalid = "WebsiteUrlInvalid";
    private const string RecruitmentUrlInvalid = "RecruitmentUrlInvalid";
    private const string DescriptionMaxLength = "DescriptionMaxLength";
    private const string ContactNoteMaxLength = "ContactNoteMaxLength";
    private const string AccountingSystemClientIdMaxLength = "AccountingSystemClientIdMaxLength";
    private const string CompanyActivityMaxLength = "CompanyActivityMaxLength";

    private readonly IClientRepository clientRepository;

    public ClientValidator(IClientRepository clientRepository)
    {
        this.clientRepository = clientRepository ?? throw new ArgumentNullException(nameof(clientRepository));

        RuleFor(x => x)
            .SetValidator(new NullInputValidator<ClientValidatedObject>())
            .DependentRules(() =>
            {
                When(_ => Id.HasValue, () =>
                    {
                        RuleFor(x => Id)
                            .HasExistingEntity(clientRepository)
                            .DependentRules(SetValidationRules);
                    })
                    .Otherwise(SetValidationRules);
            });
    }

    public int? Id { get; init; }

    private static bool BeValidUrl(string url) =>
        Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
        && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

    private void SetValidationRules()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithErrorCode(NameNotEmpty)
            .MaximumLength(100).WithErrorCode(NameMaxLength);

        RuleFor(x => x.TradeId)
            .NotEmpty().WithErrorCode(TradeIdNotEmpty)
            .MaximumLength(50).WithErrorCode(TradeIdMaxLength)
            .MustAsync(BeUniqueTradeId).WithErrorCode(TradeIdAlreadyExists).WithMessage(ClientWithSameTradeIdAlreadyExists);

        RuleFor(x => x.VatId)
            .MaximumLength(50).WithErrorCode(VatIdMaxLength)
            .Matches(@"^[A-Z]{2}[0-9A-Z]{8,12}$").WithErrorCode(VatIdInvalidFormat)
            .MustAsync(BeUniqueVatId).WithErrorCode(VatIdAlreadyExists).WithMessage(ClientWithSameVatNumberAlreadyExists);

        RuleFor(x => x.Representative)
            .NotEmpty().WithErrorCode(RepresentativeNotEmpty)
            .MaximumLength(100).WithErrorCode(RepresentativeMaxLength);

        RuleFor(x => x.NumberOfEmployees)
            .GreaterThanOrEqualTo(0).WithErrorCode(EmployeesGreaterThanOrEqualToZero);

        RuleFor(x => x.WebsiteUrl)
            .Must(BeValidUrl)
                .When(x => !string.IsNullOrEmpty(x.WebsiteUrl))
                .WithErrorCode(WebsiteUrlInvalid)
                .WithMessage(WebsiteUrlIsNotValid);

        RuleFor(x => x.RecruitmentUrl)
            .Must(BeValidUrl)
                .When(x => !string.IsNullOrEmpty(x.RecruitmentUrl))
                .WithErrorCode(RecruitmentUrlInvalid)
                .WithMessage(RecruitmentUrlIsNotValid);

        RuleFor(x => x.ParentClientId)
            .MustAsync(ParentClientExists)
                .When(x => x.ParentClientId.HasValue)
                .WithErrorCode(ParentClientWithThisIdDoesNotExist)
                .WithMessage(ParentClientWithThisIdDoesNotExist);

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithErrorCode(DescriptionMaxLength);

        RuleFor(x => x.ContactNote)
            .MaximumLength(1000).WithErrorCode(ContactNoteMaxLength);

        RuleFor(x => x.AccountingSystemClientId)
            .MaximumLength(50).WithErrorCode(AccountingSystemClientIdMaxLength);

        RuleFor(x => x.CompanyActivity)
            .MaximumLength(200).WithErrorCode(CompanyActivityMaxLength);

        RuleFor(x => x.TaxDomicile)
            .IsInEnum().WithErrorCode(MustBeAValidTaxDomicileIsoCode).WithMessage(MustBeAValidTaxDomicileIsoCode);
    }

    private async Task<bool> BeUniqueTradeId(ClientValidatedObject model, string tradeId, CancellationToken cancellationToken)
    {
        var existingClient = await clientRepository.FindByTradeIdAsync(tradeId, cancellationToken);
        if (existingClient == null)
        {
            return true;
        }

        return Id.HasValue && existingClient.Id == Id.Value;
    }

    private async Task<bool> BeUniqueVatId(ClientValidatedObject model, string vatId, CancellationToken cancellationToken)
    {
        var existingClient = await clientRepository.FindByVatIdAsync(vatId, cancellationToken);
        if (existingClient == null)
        {
            return true;
        }

        return Id.HasValue && existingClient.Id == Id.Value;
    }

    private async Task<bool> ParentClientExists(ClientValidatedObject model, int? parentClientId, CancellationToken cancellationToken)
    {
        var parentClient = await clientRepository.GetAsync(parentClientId!.Value);
        return parentClient != null;
    }
}