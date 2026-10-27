using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Verisoft.Core.Codebooks.Common.Entities;
using Verisoft.Core.Codebooks.Common.Filters;
using Verisoft.Core.Codebooks.Common.Models;
using Verisoft.Core.Codebooks.Common.Specifications;
using Verisoft.Core.Codebooks.Constants;
using Verisoft.Core.Codebooks.Extensions;
using Verisoft.Core.Codebooks.Helpers;
using Verisoft.Core.Codebooks.Repositories;
using Verisoft.Core.Codebooks.Sorts;
using Verisoft.Core.Codebooks.Validators;
using Verisoft.Core.Common.Entities.Codebooks;
using Verisoft.Core.Contracts;
using Verisoft.Core.Contracts.BusinessResult;
using Verisoft.Core.Contracts.Validation.Extensions;

namespace Verisoft.Core.Codebooks.Services;

public class StringValueService<TCodebookRecordType>(
    ITranslationRepository<TCodebookRecordType> translationRepository,
    ICodebookRecordRepository<TCodebookRecordType> codebookRecordRepository,
    ILanguageRepository languageRepository,
    ILogger<StringValueService<TCodebookRecordType>> logger)
    : IStringValueService<TCodebookRecordType>
    where TCodebookRecordType : CodebookRecordType, new()
{
    public virtual async Task<BusinessActionResult<StringValueDetail<TCodebookRecordType>>> GetStringValueDetailAsync(
        string stringValue)
    {
        if (string.IsNullOrWhiteSpace(stringValue))
        {
            return ErrorFactory.BadRequest("String value is required");
        }

        var codebookRecordEntity = await codebookRecordRepository.GetStringValueDetailAsync(stringValue);

        if (codebookRecordEntity is null)
        {
            return ErrorFactory.NotFound<CodebookRecordEntity<TCodebookRecordType>>(nameof(CodebookRecordEntity<TCodebookRecordType>.StringValue), stringValue);
        }

        return MapResponse(codebookRecordEntity);
    }

    public async Task<BusinessActionResult<StringValueDetailListData<TCodebookRecordType>>> GetStringValueDetailListAsync(
        FilteredRequest<StringValueDetailFilter> request)
    {
        var specification = new StringValueDetailSpecification<TCodebookRecordType> { SearchTerm = request.Filter?.SearchTerm };
        var filter = specification.SatisfiedBy();

        var response = await codebookRecordRepository.GetPagedSortAsync<CodebookRecordSort<TCodebookRecordType>>(request.Offset, request.Limit, filter, request.ParsedSorts);

        return new StringValueDetailListData<TCodebookRecordType>()
        {
            Data = response.Items.Select(MapResponse),
            Total = response.Count,
        };
    }

    public virtual async Task<BusinessActionResult<StringValueDetail<TCodebookRecordType>>> AddStringValueAsync(StringValueDetailCreateModel<TCodebookRecordType> stringValueDetail)
    {
        var codebookRecordEntity = MapStringValueDetailPropertiesToEntity(stringValueDetail);

        await codebookRecordRepository.AddAsync(codebookRecordEntity);
        await AddDefaultTranslationAsync(codebookRecordEntity, stringValueDetail.DefaultTranslation);

        codebookRecordRepository.UnitOfWork.Commit();

        return await GetStringValueDetailAsync(stringValueDetail.StringValue);
    }

    public virtual async Task<BusinessActionResult<StringValueDetail<TCodebookRecordType>>> UpdateStringValueAsync(
    string stringValue,
    StringValueDetailEditModel<TCodebookRecordType> stringValueDetail)
    {
        var codebookRecordEntity = await codebookRecordRepository.GetStringValueDetailAsync(stringValue);

        if (codebookRecordEntity is null)
        {
            return ErrorFactory.NotFound<TCodebookRecordType>(nameof(stringValue), stringValue);
        }

        MapStringValueDetailPropertiesToEntity(stringValueDetail, codebookRecordEntity);

        codebookRecordRepository.Update(codebookRecordEntity);

        await UpdateDefaultTranslationAsync(codebookRecordEntity, stringValueDetail.DefaultTranslation);

        codebookRecordRepository.UnitOfWork.Commit();

        return await GetStringValueDetailAsync(stringValue);
    }

    public virtual async Task<BusinessActionResult> RemoveStringValueAsync(string stringValue)
    {
        var codebookRecordEntity = await codebookRecordRepository.GetStringValueDetailAsync(stringValue);

        if (codebookRecordEntity is null)
        {
            return ErrorFactory.NotFound<TCodebookRecordType>(nameof(stringValue), stringValue);
        }

        codebookRecordRepository.HardDeleteCodebookRecord(codebookRecordEntity);

        try
        {
            codebookRecordRepository.UnitOfWork.Commit();
            return new BusinessActionResult();
        }
        catch (DbUpdateException ex) when (ex.IsForeignKeyConstraintViolation())
        {
            logger.LogWarning(ex, "Failed to delete codebook record with string value '{StringValue}' due to foreign key constraint violation.", stringValue);
            return CodebookApiErrorFactory.ForeignKeyConstraintViolation();
        }
    }

    public virtual async Task<BusinessActionResult<StringValueDetailValidatedModel<TCodebookRecordType>>> ValidateAsync(
        string stringValue,
        StringValueDetailEditModel<TCodebookRecordType> stringValueDetail,
        bool isCreationValidation)
    {
        var stringValueDetailValidatedObject = new StringValueDetailValidatedModel<TCodebookRecordType>
        {
            Description = stringValueDetail.Description,
            DefaultTranslation = stringValueDetail.DefaultTranslation,
            AdditionalProperties = stringValueDetail.AdditionalProperties,
            ValidFrom = stringValueDetail.ValidFrom,
            ValidTo = stringValueDetail.ValidTo,
        };

        var stringValueDetailValidator = new StringValueDetailValidator<TCodebookRecordType>(codebookRecordRepository)
        {
            StringValue = stringValue,
            IsCreationValidation = isCreationValidation,
        };

        (await stringValueDetailValidator.ValidateAsync(stringValueDetailValidatedObject)).Errors?.ForEach(stringValueDetailValidatedObject.LogError);
        return stringValueDetailValidatedObject;
    }

    public virtual async Task<BusinessActionResult<StringValueTranslations>> UpsertTranslationAsync(string stringValue, string languageCode, string value)
    {
        var language = await languageRepository.GetByCodeAsync(languageCode);
        if (language is null)
        {
            return ErrorFactory.NotFound<LanguageEntity>(nameof(languageCode), languageCode);
        }

        var stringValueDetail = await codebookRecordRepository.GetStringValueDetailAsync(stringValue);
        if (stringValueDetail is null)
        {
            return ErrorFactory.NotFound<TCodebookRecordType>(nameof(stringValue), stringValue);
        }

        var translation = stringValueDetail.Translations.FirstOrDefault(x => x.LanguageCode == languageCode);
        if (translation is null)
        {
            translation = new CodebookRecordTranslationEntity<TCodebookRecordType>
            {
                LanguageCode = languageCode,
                Value = value,
                CodebookRecord = stringValueDetail,
            };
            await translationRepository.AddAsync(translation);
        }
        else
        {
            translation.Value = value;
            translationRepository.Update(translation);
        }

        translationRepository.UnitOfWork.Commit();
        stringValueDetail = await codebookRecordRepository.GetStringValueDetailAsync(stringValue);
        return new StringValueTranslations
        {
            StringValue = stringValueDetail!.StringValue,
            Translations = [.. stringValueDetail.Translations.Select(x => new Translation { LanguageCode = x.LanguageCode, Value = x.Value })],
        };
    }

    public virtual async Task<BusinessActionResult<StringValueTranslations>> RemoveTranslationAsync(string stringValue, string languageCode)
    {
        var stringValueDetail = await codebookRecordRepository.GetStringValueDetailAsync(stringValue);
        if (stringValueDetail is null)
        {
            return ErrorFactory.NotFound<TCodebookRecordType>(nameof(stringValue), stringValue);
        }

        var translation = stringValueDetail.Translations.FirstOrDefault(x => string.Equals(x.LanguageCode, languageCode, StringComparison.OrdinalIgnoreCase));
        if (translation is null)
        {
            return ErrorFactory.NotFound<CodebookRecordTranslationEntity<TCodebookRecordType>>(nameof(languageCode), languageCode);
        }

        if (string.Equals(translation.LanguageCode, LanguageConstants.DefaultLanguageCode, StringComparison.OrdinalIgnoreCase))
        {
            return ErrorFactory.BadRequest("Default translation cannot be removed");
        }

        translationRepository.Remove(translation);
        translationRepository.UnitOfWork.Commit();

        stringValueDetail = await codebookRecordRepository.GetStringValueDetailAsync(stringValue);
        return new StringValueTranslations
        {
            StringValue = stringValueDetail!.StringValue,
            Translations = [.. stringValueDetail.Translations.Select(x => new Translation { LanguageCode = x.LanguageCode, Value = x.Value })],
        };
    }

    protected virtual CodebookRecordEntity<TCodebookRecordType> MapStringValueDetailPropertiesToEntity(StringValueDetailCreateModel<TCodebookRecordType> stringValueDetail)
    {
        return new CodebookRecordEntity<TCodebookRecordType>
        {
            StringValue = stringValueDetail.StringValue,
            Description = stringValueDetail.Description,
            ValidFrom = stringValueDetail.ValidFrom,
            ValidTo = stringValueDetail.ValidTo,
            AdditionalProperties = stringValueDetail.AdditionalProperties ?? new(),
        };
    }

    protected virtual void MapStringValueDetailPropertiesToEntity(StringValueDetailEditModel<TCodebookRecordType> stringValueDetail, CodebookRecordEntity<TCodebookRecordType> entity)
    {
        entity.Description = stringValueDetail.Description;
        entity.ValidFrom = stringValueDetail.ValidFrom;
        entity.ValidTo = stringValueDetail.ValidTo;
        entity.AdditionalProperties = stringValueDetail.AdditionalProperties ?? new();
    }

    protected virtual StringValueDetail<TCodebookRecordType> MapResponse(
        CodebookRecordEntity<TCodebookRecordType> codebookRecordEntity)
    {
        var response = new StringValueDetail<TCodebookRecordType>()
        {
            Id = codebookRecordEntity.Id,
            StringValue = codebookRecordEntity.StringValue,
            Description = codebookRecordEntity.Description,
            AdditionalProperties = codebookRecordEntity.AdditionalProperties,
            DefaultTranslation = codebookRecordEntity.Translations.First(x => x.LanguageCode == LanguageConstants.DefaultLanguageCode).Value,
            IsValid = codebookRecordEntity.IsValid,
            ValidFrom = codebookRecordEntity.ValidFrom,
            ValidTo = codebookRecordEntity.ValidTo,
        };

        return response;
    }

    protected virtual async Task AddDefaultTranslationAsync(CodebookRecordEntity<TCodebookRecordType> codebookRecordEntity, string defaultTranslation)
    {
        var translationEntity = new CodebookRecordTranslationEntity<TCodebookRecordType>
        {
            Value = defaultTranslation,
            LanguageCode = LanguageConstants.DefaultLanguageCode,
            CodebookRecord = codebookRecordEntity,
        };

        await translationRepository.AddAsync(translationEntity);
    }

    protected virtual async Task UpdateDefaultTranslationAsync(CodebookRecordEntity<TCodebookRecordType> codebookRecordEntity, string defaultTranslation)
    {
        var defaultTranslationEntity = await translationRepository.GetTranslationByRecordIdAndLanguageCodeAsync(codebookRecordEntity.Id, LanguageConstants.DefaultLanguageCode);

        if (defaultTranslationEntity is null)
        {
            await AddDefaultTranslationAsync(codebookRecordEntity, defaultTranslation);
            return;
        }

        if (defaultTranslationEntity.Value != defaultTranslation)
        {
            defaultTranslationEntity.Value = defaultTranslation;
            translationRepository.Update(defaultTranslationEntity);
        }
    }
}