using FluentValidation;
using Verisoft.Core.Codebooks.Common.Models;
using Verisoft.Core.Codebooks.Constants;
using Verisoft.Core.Codebooks.Repositories;
using Verisoft.Core.Common.Constants;
using Verisoft.Core.Common.Entities.Codebooks;

namespace Verisoft.Core.Codebooks.Validators;

public class StringValueDetailValidator<TCodebookRecordType> : AbstractValidator<StringValueDetailValidatedModel<TCodebookRecordType>>
    where TCodebookRecordType : CodebookRecordType, new()
{
    private readonly ICodebookRecordRepository<TCodebookRecordType> codebookRecordRepository;

    public StringValueDetailValidator(ICodebookRecordRepository<TCodebookRecordType> codebookRecordRepository)
    {
        this.codebookRecordRepository = codebookRecordRepository ?? throw new ArgumentNullException(nameof(codebookRecordRepository));

        RuleFor(_ => StringValue)
            .NotEmpty()
            .WithErrorCode(nameof(ValidationConstants.FieldRequired))
            .WithMessage(ValidationConstants.FieldRequired)
            .DependentRules(() =>
            {
                RuleFor(_ => StringValue)
                    .MustAsync(async (stringValue, cancellationToken) => await this.codebookRecordRepository.GetStringValueDetailAsync(stringValue) is not null)
                    .When(_ => !IsCreationValidation)
                    .WithErrorCode(nameof(ValidationConstants.EntityNotFound))
                    .WithMessage(ValidationConstants.EntityNotFound)
                    .DependentRules(() =>
                    {
                        RuleFor(_ => StringValue).Cascade(CascadeMode.Stop)
                             .MaximumLength(CodebookRecordEntityConstants.StringValueMaxLength)
                             .WithErrorCode(nameof(ValidationConstants.StringFieldTooLong))
                             .WithMessage(string.Format(ValidationConstants.StringFieldTooLong, CodebookRecordEntityConstants.StringValueMaxLength))

                             .MustAsync(async (stringValue, cancellationToken) => await this.codebookRecordRepository.GetStringValueDetailAsync(stringValue) is null)
                             .When(_ => IsCreationValidation, ApplyConditionTo.CurrentValidator)
                             .WithErrorCode(nameof(ValidationConstants.ValueNotUnique))
                             .WithMessage(ValidationConstants.ValueNotUnique);

                        RuleFor(x => x.Description)
                            .MaximumLength(CodebookRecordEntityConstants.DescriptionValueMaxLength)
                            .WithErrorCode(nameof(ValidationConstants.StringFieldTooLong))
                            .WithMessage(string.Format(ValidationConstants.StringFieldTooLong, CodebookRecordEntityConstants.DescriptionValueMaxLength));

                        RuleFor(x => x.DefaultTranslation)
                            .NotEmpty()
                            .WithErrorCode(nameof(ValidationConstants.FieldRequired))
                            .WithMessage(ValidationConstants.FieldRequired);

                        RuleFor(x => x.ValidTo)
                            .Must((validatedObject, validTo) => validatedObject.ValidFrom <= validTo)
                            .WithErrorCode(nameof(ValidationConstants.DateToBeforeDateFrom))
                            .WithMessage(ValidationConstants.DateToBeforeDateFrom)
                            .When(x => x.ValidTo.HasValue);
                    });
            });
    }

    public string StringValue { get; init; } = string.Empty;

    public bool IsCreationValidation { get; init; } = false;
}
