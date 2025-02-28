using FluentValidation;
using Verisoft.CodebookApi.Application.Helpers;
using Verisoft.CodebookApi.Contracts.Models.Language;
using Verisoft.CodebookApi.Core.Repositories;
using Verisoft.Core.Validation.Validators;

namespace Verisoft.CodebookApi.Application.Validators;

public class LanguageValidator : AbstractValidator<LanguageValidatedModel>
{
    public LanguageValidator(ILanguageRepository languageRepository)
    {
        RuleFor(x => x)
            .SetValidator(new NullInputValidator<LanguageValidatedModel>())
            .DependentRules(() =>
            {
                RuleFor(_ => Id)
                .HasExistingEntity(languageRepository)
                .When(_ => Id.HasValue)
                .DependentRules(() =>
                {
                    RuleFor(x => x.Name)
                        .NotEmpty()
                        .WithErrorCode(CodebookApiErrorFactory.LanguageNameNotSetErrorCode)
                        .WithMessage(CodebookApiErrorFactory.LanguageNameNotSetErrorMessage)
                        .DependentRules(() =>
                        {
                            RuleFor(x => x.Name)
                                .MustAsync(async (name, cancellationToken) =>
                                {
                                    var languageEntities = await languageRepository.GetAll();
                                    return !languageEntities.Any(x => x.Name == name && (!Id.HasValue || x.Id != Id));
                                })
                                .WithErrorCode(CodebookApiErrorFactory.LanguageAlreadyExistErrorCode)
                                .WithMessage(CodebookApiErrorFactory.LanguageAlreadyExistErrorMessage);
                        });

                    RuleFor(x => x.Code)
                    .NotEmpty()
                        .WithErrorCode(CodebookApiErrorFactory.LanguageCodeNotSetErrorCode)
                        .WithMessage(CodebookApiErrorFactory.LanguageCodeNotSetErrorMessage)
                        .DependentRules(() =>
                        {
                            RuleFor(x => x.Code)
                                .MustAsync(async (code, cancellationToken) =>
                                {
                                    var languageEntities = await languageRepository.GetAll();
                                    return !languageEntities.Any(x => x.Code == code && (!Id.HasValue || x.Id != Id));
                                })
                                .WithErrorCode(CodebookApiErrorFactory.LanguageCodeAlreadyExistErrorCode)
                                .WithMessage(CodebookApiErrorFactory.LanguageCodeAlreadyExistErrorMessage);
                        });
                });
            });
    }

    public int? Id { get; set; }
}