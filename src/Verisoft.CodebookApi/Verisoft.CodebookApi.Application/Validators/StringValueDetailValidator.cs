using FluentValidation;
using Verisoft.CodebookApi.Application.Helpers;
using Verisoft.CodebookApi.Contracts.Models.Base;
using Verisoft.CodebookApi.Contracts.Models.CodebookRecord;
using Verisoft.CodebookApi.Core.Entities.BaseEntity;
using Verisoft.CodebookApi.Core.Repositories;

namespace Verisoft.CodebookApi.Application.Validators;

public class StringValueDetailValidator<TAdditionalProperties, TTenantValue, TCodebookRecordEntity> : AbstractValidator<StringValueDetailValidatedModel<TAdditionalProperties, TTenantValue>>
    where TAdditionalProperties : class, new()
    where TTenantValue : class, ITenantValue, new()
    where TCodebookRecordEntity : class, ICodebookRecordEntity, new()
{
    public StringValueDetailValidator(string stringValue, bool? isGlobal, bool isCreationValidation, ICodebookRecordRepository<TCodebookRecordEntity> codebookRecordRepository, ITenantRepository tenantRepository)
    {
        RuleFor(_ => stringValue)
            .NotEmpty()
            .WithErrorCode(CodebookApiErrorFactory.StringValueNotSetErrorCode)
            .WithMessage(CodebookApiErrorFactory.StringValueNotSetErrorMessage)
            .DependentRules(() =>
            {
                RuleFor(_ => stringValue)
                    .MustAsync(async (stringValue, cancellationToken) => (await codebookRecordRepository.GetCodebookRecordsAsync(stringValue)).Count > 0)
                    .When(_ => !isCreationValidation)
                    .WithErrorCode(CodebookApiErrorFactory.StringValueDoesNotExistErrorCode)
                    .WithMessage(CodebookApiErrorFactory.StringValueDoesNotExistErrorMessage)
                    .DependentRules(() =>
                    {
                        RuleFor(_ => stringValue).Cascade(CascadeMode.Stop)
                                .Must(stringValue => char.IsAsciiLetter(stringValue[0]))
                                .When(_ => isCreationValidation)
                                .WithErrorCode(CodebookApiErrorFactory.StringValueDoesNotStartWithLetterErrorCode)
                                .WithMessage(CodebookApiErrorFactory.StringValueDoesNotStartWithLetterErrorMessage)

                                .MaximumLength(255)
                                .WithErrorCode(CodebookApiErrorFactory.StringValueTooLongErrorCode)
                                .WithMessage(CodebookApiErrorFactory.StringValueTooLongErrorMessage)

                                .MustAsync(async (stringValue, cancellationToken) => (await codebookRecordRepository.GetCodebookRecordsAsync(stringValue)).Count is 0)
                                .When(_ => isCreationValidation)
                                .WithErrorCode(CodebookApiErrorFactory.StringValueAlreadyExistErrorCode)
                                .WithMessage(CodebookApiErrorFactory.StringValueAlreadyExistErrorMessage);

                        RuleFor(x => x.Description)
                            .MaximumLength(255)
                            .WithErrorCode(CodebookApiErrorFactory.DescriptionTooLongErrorCode)
                            .WithMessage(CodebookApiErrorFactory.DescriptionTooLongErrorMessage);

                        RuleFor(x => x.TenantValues)
                            .NotEmpty()
                            .WithErrorCode(CodebookApiErrorFactory.LocalTenantValuesNotSetErrorCode)
                            .WithMessage(CodebookApiErrorFactory.LocalTenantValuesNotSetErrorMessage)
                            .WhenAsync(async (_, cancellationToken) =>
                            {
                                if (isGlobal.HasValue)
                                {
                                    return !isGlobal.Value;
                                }

                                var entity = await codebookRecordRepository.GetCodebookRecordsAsync(stringValue);
                                return !entity.First().IsGlobal;
                            })
                            .DependentRules(() =>
                                RuleFor(x => x.TenantValues)
                                .Must(tenantValues => tenantValues?.Count is 1)
                                .WithErrorCode(CodebookApiErrorFactory.LocalTenantValuesNotSingleSingleTenantValueErrorCode)
                                .WithMessage(CodebookApiErrorFactory.LocalTenantValuesNotSingleSingleTenantValueErrorMessage)
                                .WhenAsync(async (_, cancellationToken) =>
                                {
                                    if (isGlobal.HasValue)
                                    {
                                        return !isGlobal.Value;
                                    }

                                    var entity = await codebookRecordRepository.GetCodebookRecordsAsync(stringValue);
                                    return !entity.First().IsGlobal;
                                }))

                            .MustAsync(async (tenantValues, cancellationToken) =>
                            {
                                var allTenantIds = await tenantRepository.GetAllTenantIdsAsync();
                                return tenantValues!.All(tv => allTenantIds.Contains(tv.TenantId));
                            })
                            .WithErrorCode(CodebookApiErrorFactory.TenantsNotExistErrorCode)
                            .WithMessage(CodebookApiErrorFactory.TenantsNotExistErrorMessage)
                            .When(x => x.TenantValues?.Count is not null and not 0)

                            .Must(tenantValues => tenantValues!.GroupBy(tv => tv.TenantId).All(tvg => tvg.Count() is 1))
                            .WithErrorCode(CodebookApiErrorFactory.TenantValuesNotUniqueErrorCode)
                            .WithMessage(CodebookApiErrorFactory.TenantValuesNotUniqueErrorMessage)
                            .When(x => x.TenantValues?.Count is not null and not 0);

                        RuleFor(x => x.DefaultTranslation)
                            .NotEmpty()
                            .WithErrorCode(CodebookApiErrorFactory.DefaultLanguageTranslationNotSetErrorCode)
                            .WithMessage(CodebookApiErrorFactory.DefaultLanguageTranslationNotSetErrorMessage);
                    });
            });
    }
}
