using FluentValidation;
using Verisoft.Core.Codebooks.Common.Models;
using Verisoft.Core.Codebooks.Constants;
using Verisoft.Core.Codebooks.Repositories;
using Verisoft.Core.Validation.Validators;

namespace Verisoft.Core.Codebooks.Validators;

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
                        .WithErrorCode(nameof(ValidationConstants.FieldRequired))
                        .WithMessage(ValidationConstants.FieldRequired)
                        .DependentRules(() =>
                        {
                            RuleFor(x => x.Name)
                                .MustAsync(async (name, cancellationToken) =>
                                {
                                    var languageEntities = await languageRepository.GetAllAsync();
                                    return !languageEntities.Any(x => x.Name == name && (!Id.HasValue || x.Id != Id));
                                })
                                .WithErrorCode(nameof(ValidationConstants.ValueNotUnique))
                                .WithMessage(ValidationConstants.ValueNotUnique);
                        });

                    RuleFor(x => x.Code)
                    .NotEmpty()
                        .WithErrorCode(nameof(ValidationConstants.FieldRequired))
                        .WithMessage(ValidationConstants.FieldRequired)
                        .DependentRules(() =>
                        {
                            RuleFor(x => x.Code)
                                .MustAsync(async (code, cancellationToken) =>
                                {
                                    var languageEntities = await languageRepository.GetAllAsync();
                                    return !languageEntities.Any(x => x.Code == code && (!Id.HasValue || x.Id != Id));
                                })
                                .WithErrorCode(nameof(ValidationConstants.ValueNotUnique))
                                .WithMessage(ValidationConstants.ValueNotUnique);
                        });
                });
            });
    }

    public int? Id { get; set; }
}