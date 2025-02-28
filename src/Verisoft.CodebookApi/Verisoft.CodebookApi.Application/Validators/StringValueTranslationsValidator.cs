using FluentValidation;
using Verisoft.CodebookApi.Application.Helpers;
using Verisoft.CodebookApi.Contracts.Models.Translation;
using Verisoft.CodebookApi.Core.Constants;
using Verisoft.CodebookApi.Core.Repositories;
using Verisoft.Core.Validation.Validators;

namespace Verisoft.CodebookApi.Application.Validators;

public class StringValueTranslationsValidator : AbstractValidator<StringValueTranslationsValidatedModel>
{
    public StringValueTranslationsValidator(ILanguageRepository languageRepository)
    {
        RuleFor(x => x)
            .SetValidator(new NullInputValidator<StringValueTranslationsValidatedModel>())
            .DependentRules(() =>
            {
                RuleFor(x => x.Translations)
                    .NotEmpty()
                    .WithErrorCode(CodebookApiErrorFactory.TranslationsNotSetErrorCode)
                    .WithMessage(CodebookApiErrorFactory.TranslationsNotSetErrorMessage)
                    .DependentRules(() =>
                        RuleFor(x => x.Translations)
                            .Must(translations => translations.GroupBy(x => x.LanguageId).All(g => g.Count() == 1))
                            .WithErrorCode(CodebookApiErrorFactory.TranslationLanguagesNotUniqueErrorCode)
                            .WithErrorCode(CodebookApiErrorFactory.TranslationLanguagesNotUniqueErrorMessage)

                            .MustAsync(async (translations, cancellationToken) =>
                            {
                                var languages = await languageRepository.GetAll();
                                var languageIds = languages.Select(x => x.Id);
                                return translations.All(x => languageIds.Contains(x.LanguageId));
                            })
                            .WithErrorCode(CodebookApiErrorFactory.LanguageDoesNotExistErrorCode)
                            .WithMessage(CodebookApiErrorFactory.LanguageDoesNotExistErrorMessage)

                            .Must(translations => translations.Any(x => x.LanguageId == LanguageConstants.DefaultLanguageId))
                            .WithErrorCode(CodebookApiErrorFactory.DefaultLanguageTranslationNotSetErrorCode)
                            .WithMessage(CodebookApiErrorFactory.DefaultLanguageTranslationNotSetErrorMessage));
            });
    }
}
