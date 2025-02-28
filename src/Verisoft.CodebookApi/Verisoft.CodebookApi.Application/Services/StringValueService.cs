using FluentValidation;
using Verisoft.CodebookApi.Application.Helpers;
using Verisoft.CodebookApi.Application.Services.Interfaces;
using Verisoft.CodebookApi.Application.Validators;
using Verisoft.CodebookApi.Contracts.Filters;
using Verisoft.CodebookApi.Contracts.Models.Base;
using Verisoft.CodebookApi.Contracts.Models.CodebookRecord;
using Verisoft.CodebookApi.Core.Constants;
using Verisoft.CodebookApi.Core.Entities;
using Verisoft.CodebookApi.Core.Entities.BaseEntity;
using Verisoft.CodebookApi.Core.Repositories;
using Verisoft.Core.Authentication;
using Verisoft.Core.Contracts;
using Verisoft.Core.Contracts.BusinessResult;
using Verisoft.Core.Contracts.Validation.Extensions;
using CommonObjectReflectionHelper = Verisoft.Core.Common.Helpers.ObjectReflectionHelper;

namespace Verisoft.CodebookApi.Application.Services;

public class StringValueService<TCodebookRecordEntity>(
    ITranslationRepository translationRepository,
    ICodebookRecordRepository<TCodebookRecordEntity> codebookRecordRepository,
    ITenantValueRepository tenantValueRepository,
    ITenantRepository tenantRepository,
    ILanguageRepository languageRepository,
    IUserContext userContext)
    : IStringValueService<TCodebookRecordEntity>
    where TCodebookRecordEntity : class, ICodebookRecordEntity, new()
{
    private const bool IsForbidden = true;

    private readonly string tableName = typeof(TCodebookRecordEntity).Name.Replace("Entity", string.Empty);
    private readonly ICodebookRecordRepository<TCodebookRecordEntity> codebookRecordRepository = codebookRecordRepository ?? throw new ArgumentNullException(nameof(codebookRecordRepository));
    private readonly ITranslationRepository translationRepository = translationRepository ?? throw new ArgumentNullException(nameof(translationRepository));
    private readonly ITenantValueRepository tenantValueRepository = tenantValueRepository ?? throw new ArgumentNullException(nameof(tenantValueRepository));
    private readonly ITenantRepository tenantRepository = tenantRepository ?? throw new ArgumentNullException(nameof(tenantRepository));
    private readonly ILanguageRepository languageRepository = languageRepository ?? throw new ArgumentNullException(nameof(languageRepository));
    private readonly IUserContext userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));

    public async Task<BusinessActionResult<StringValueDetail<TAdditionalProperties, TTenantValue>>> GetStringValueDetailAsync<TAdditionalProperties, TTenantValue>(
        string stringValue)
    where TAdditionalProperties : class, new()
    where TTenantValue : class, ITenantValue, new()
    {
        if (string.IsNullOrWhiteSpace(stringValue))
        {
            return ErrorFactory.BadRequest("String value is required");
        }

        var codebookRecordEntities = await codebookRecordRepository.GetCodebookRecordsAsync(stringValue);

        if (codebookRecordEntities.Count is 0)
        {
            return ErrorFactory.NotFound(typeof(TCodebookRecordEntity).Name, "stringValue", stringValue);
        }

        return await CreateResponseAsync<TAdditionalProperties, TTenantValue>(codebookRecordEntities);
    }

    public async Task<BusinessActionResult<StringValueDetailListData<TAdditionalProperties, TTenantValue>>> GetStringValueDetailListAsync<TAdditionalProperties, TTenantValue>(
        FilteredRequest<StringValueDetailFilter> request)
        where TAdditionalProperties : class, new()
        where TTenantValue : class, ITenantValue, new()
    {
        request.Filter ??= new();
        var groupedCodebookRecordEntities = (await codebookRecordRepository.GetCodebookRecordsAsync(searchTerm: request.Filter.SearchTerm)).GroupBy(e => e.StringValue);
        groupedCodebookRecordEntities = GetFilteredCodebookRecordGroups(groupedCodebookRecordEntities, request.Filter);

        var responseList = await CreateResponseListAsync<TAdditionalProperties, TTenantValue>(groupedCodebookRecordEntities);

        return new StringValueDetailListData<TAdditionalProperties, TTenantValue>()
        {
            Data = GetOrderedStringValueDetails(request.Sort, responseList).Skip(request.Offset).Take(request.Limit),
            Total = responseList.Count,
        };
    }

    public async Task<BusinessActionResult<StringValueDetail<TAdditionalProperties, TTenantValue>>> AddStringValueAsync<TAdditionalProperties, TTenantValue>(StringValueDetail<TAdditionalProperties, TTenantValue> stringValueDetail)
        where TAdditionalProperties : class, new()
        where TTenantValue : class, ITenantValue, new()
    {
        await (stringValueDetail.IsGlobal ? AddGlobalStringValueAsync(stringValueDetail) : AddLocalStringValueAsync(stringValueDetail));

        await AddDefaultTranslationAsync(stringValueDetail.StringValue, stringValueDetail.DefaultTranslation);

        var codebookRecordEntitiesForResponse = await codebookRecordRepository.GetCodebookRecordsAsync(stringValueDetail.StringValue);
        return await CreateResponseAsync<TAdditionalProperties, TTenantValue>(codebookRecordEntitiesForResponse);
    }

    public async Task<BusinessActionResult<StringValueDetailListData<TAdditionalProperties, TTenantValue>>> AddStringValuesInBulkAsync<TAdditionalProperties, TTenantValue>(
        StringValueBulkAddModel<TAdditionalProperties, TTenantValue> stringValueBulkAddModel,
        string codebookRecordTableName)
        where TAdditionalProperties : class, new()
        where TTenantValue : class, ITenantValue, new()
    {
        var stringValueDetailList = new List<StringValueDetail<TAdditionalProperties, TTenantValue>>();
        var groupedStringValues = stringValueBulkAddModel.StringValues.GroupBy(sv => sv.StringValue);
        var errors = new BusinessActionErrors();

        if (stringValueBulkAddModel.StringValues.Any(sv => string.IsNullOrWhiteSpace(sv.StringValue)))
        {
            errors.Add(CodebookApiErrorFactory.EmptyStringValue());
        }

        errors.AddRange(groupedStringValues.Where(g => g.Count() > 1).Select(g => CodebookApiErrorFactory.DuplicitStringValue(g.Key)));

        if (errors.HasErrors)
        {
            return errors;
        }

        foreach (var stringValue in stringValueBulkAddModel.StringValues)
        {
            var stringValueDetail = new StringValueDetail<TAdditionalProperties, TTenantValue>
            {
                DefaultAdditionalProperties = new(),
                TenantValues = [],
                StringValue = stringValue.StringValue,
                DefaultTranslation = stringValue.DefaultTranslation,
            };
            var addResult = await AddStringValueAsync(stringValueDetail);

            if (addResult.IsSuccess)
            {
                stringValueDetailList.Add(addResult.Data);
            }
        }

        return new StringValueDetailListData<TAdditionalProperties, TTenantValue>
        {
            Data = stringValueDetailList,
            Total = stringValueDetailList.Count,
        };
    }

    public async Task<BusinessActionResult<StringValueDetail<TAdditionalProperties, TTenantValue>>> UpdateStringValueAsync<TAdditionalProperties, TTenantValue>(
        string stringValue,
        StringValueDetailEditModel<TAdditionalProperties, TTenantValue> stringValueDetail)
        where TAdditionalProperties : class, new()
        where TTenantValue : class, ITenantValue, new()
    {
        var codebookRecordEntities = await codebookRecordRepository.GetCodebookRecordsAsync(stringValue);

        if (codebookRecordEntities?.Count is null or 0)
        {
            return ErrorFactory.NotFound<TCodebookRecordEntity>(nameof(stringValue), stringValue);
        }

        if (codebookRecordEntities.Any(x => x.IsGlobal))
        {
            var defaultCodebookRecordEntity = codebookRecordEntities.First(e => e.IsDefault);
            await UpdateDefaultCodebookRecord(stringValueDetail, defaultCodebookRecordEntity);
            await UpdateTenantValuesToBeForbidden<TAdditionalProperties, TTenantValue>(stringValueDetail.TenantValues, codebookRecordEntities);
            await UpdateTenantValuesNotToBeForbiddenForDefaultRecord<TAdditionalProperties, TTenantValue>(stringValueDetail.TenantValues, codebookRecordEntities, defaultCodebookRecordEntity.Id);
            await UpdateTenantValuesNotToBeForbiddenForNonDefaultRecords(stringValueDetail, codebookRecordEntities);
        }
        else
        {
            var codebookRecordEntity = codebookRecordEntities.First();
            var tenantValue = stringValueDetail.TenantValues!.First();

            codebookRecordEntity.Description = stringValueDetail.Description;
            CommonObjectReflectionHelper.MapProperties(tenantValue, codebookRecordEntity);

            await codebookRecordRepository.UpdateAsync(codebookRecordEntity);
        }

        await UpdateDefaultTranslationAsync(stringValue, stringValueDetail.DefaultTranslation);

        codebookRecordRepository.UnitOfWork.Commit();

        var codebookRecordEntitiesForResponse = await codebookRecordRepository.GetCodebookRecordsAsync(stringValue);

        return await CreateResponseAsync<TAdditionalProperties, TTenantValue>(codebookRecordEntitiesForResponse);
    }

    public async Task<BusinessActionResult<StringValueDetail<TAdditionalProperties, TTenantValue>>> RemoveStringValueAsync<TAdditionalProperties, TTenantValue>(string stringValue)
        where TAdditionalProperties : class, new()
        where TTenantValue : class, ITenantValue, new()
    {
        var codebookRecordEntities = await codebookRecordRepository.GetCodebookRecordsAsync(stringValue);

        if (codebookRecordEntities?.Count is null or 0)
        {
            return ErrorFactory.NotFound(nameof(TCodebookRecordEntity), "stringValue", stringValue);
        }

        foreach (var tenantValueEntity in codebookRecordEntities.SelectMany(x => x.TenantValues!))
        {
            if (!tenantValueEntity.IsForbidden)
            {
                tenantValueEntity.IsForbidden = true;
                await tenantValueRepository.UpdateAsync(tenantValueEntity);
            }
        }

        tenantValueRepository.UnitOfWork.Commit();

        return await CreateResponseAsync<TAdditionalProperties, TTenantValue>(codebookRecordEntities);
    }

    public async Task<BusinessActionResult<StringValueDetailValidatedModel<TAdditionalProperties, TTenantValue>>> ValidateAsync<TAdditionalProperties, TTenantValue>(
        string stringValue,
        bool? isGlobal,
        StringValueDetailEditModel<TAdditionalProperties, TTenantValue> stringValueDetail,
        bool isCreationValidation)
        where TAdditionalProperties : class, new()
        where TTenantValue : class, ITenantValue, new()
    {
        var stringValueDetailValidatedObject = new StringValueDetailValidatedModel<TAdditionalProperties, TTenantValue>
        {
            Description = stringValueDetail.Description,
            DefaultTranslation = stringValueDetail.DefaultTranslation,
            TenantValues = stringValueDetail.TenantValues,
        };

        var stringValueDetailValidator = new StringValueDetailValidator<TAdditionalProperties, TTenantValue, TCodebookRecordEntity>(stringValue, isGlobal, isCreationValidation, codebookRecordRepository, tenantRepository);
        (await stringValueDetailValidator.ValidateAsync(stringValueDetailValidatedObject)).Errors?.ForEach(stringValueDetailValidatedObject.LogError);
        return stringValueDetailValidatedObject;
    }

    private static IEnumerable<IGrouping<string, TCodebookRecordEntity>> GetExceptionalValueRecordGroups(IEnumerable<IGrouping<string, TCodebookRecordEntity>> groupedCodebookRecordEntities)
    {
        return groupedCodebookRecordEntities
                    .Where(g => g.Any(record => record.IsGlobal))
                    .Where(g =>
                    {
                        var defaultRecord = g.First(record => record.IsDefault);
                        return g.Any(record => record.TenantValues!.Any(tv => !tv.IsForbidden && tv.CodebookRecordId != defaultRecord.Id));
                    });
    }

    private static IEnumerable<IGrouping<string, TCodebookRecordEntity>> GetFilteredCodebookRecordGroups(IEnumerable<IGrouping<string, TCodebookRecordEntity>> groupedCodebookRecordEntities, StringValueDetailFilter filter)
    {
        if (filter.TenantId.HasValue)
        {
            groupedCodebookRecordEntities = groupedCodebookRecordEntities.Where(g => g.Any(record => record.TenantValues!.Any(tv => tv.TenantId == filter.TenantId.Value && !tv.IsForbidden)));
        }

        if (filter.HasExceptionalValues ?? false)
        {
            groupedCodebookRecordEntities = GetExceptionalValueRecordGroups(groupedCodebookRecordEntities);
        }

        return groupedCodebookRecordEntities;
    }

    private static List<StringValueDetail<TAdditionalProperties, TTenantValue>> GetOrderedStringValueDetails<TAdditionalProperties, TTenantValue>(
        SortDefinition sort,
        List<StringValueDetail<TAdditionalProperties, TTenantValue>> responseList)
        where TAdditionalProperties : class, new()
        where TTenantValue : class, ITenantValue, new()
    {
        sort ??= new();
        var sortField = sort.Field?.ToLower();
        var sortingExpression = sortField switch
        {
            "stringvalue" => x => x.StringValue,
            "description" => x => string.IsNullOrWhiteSpace(x.Description) ? string.Empty : x.Description,
            _ => (Func<StringValueDetail<TAdditionalProperties, TTenantValue>, string>)(x => x.StringValue),
        };

        return sort.Direction == SortDirection.Asc
            ? [.. responseList.OrderBy(sortingExpression)]
            : [.. responseList.OrderByDescending(sortingExpression)];
    }

    private static TCodebookRecordEntity MapStringValueDetailPropertiesToEntity<TAdditionalProperties, TTenantValue>(StringValueDetailEditModel<TAdditionalProperties, TTenantValue> stringValueDetail)
       where TAdditionalProperties : class, new()
       where TTenantValue : class, ITenantValue, new()
    {
        ArgumentNullException.ThrowIfNull(stringValueDetail);

        var entity = Activator.CreateInstance<TCodebookRecordEntity>();

        CommonObjectReflectionHelper.MapProperties(stringValueDetail, entity);

        if (stringValueDetail.DefaultAdditionalProperties is not null)
        {
            CommonObjectReflectionHelper.MapProperties(stringValueDetail.DefaultAdditionalProperties, entity);
        }

        return entity;
    }

    private static void MapStringValueDetailPropertiesToEntity<TAdditionalProperties, TTenantValue>(StringValueDetailEditModel<TAdditionalProperties, TTenantValue> stringValueDetail, TCodebookRecordEntity entity)
        where TAdditionalProperties : class, new()
        where TTenantValue : class, ITenantValue, new()
    {
        ArgumentNullException.ThrowIfNull(stringValueDetail);

        CommonObjectReflectionHelper.MapProperties(stringValueDetail, entity);

        if (stringValueDetail.DefaultAdditionalProperties is not null)
        {
            CommonObjectReflectionHelper.MapProperties(stringValueDetail.DefaultAdditionalProperties, entity);
        }
    }

    private static TAdditionalProperties? GetDefaultAdditionalProperties<TAdditionalProperties>(TCodebookRecordEntity? codebookRecordEntity)
        where TAdditionalProperties : class, new()
    {
        if (codebookRecordEntity is null)
        {
            return null;
        }

        var contract = new TAdditionalProperties();

        var entityProperties = codebookRecordEntity.GetType().GetProperties();

        var contractProperties = typeof(TAdditionalProperties).GetProperties();

        var matchingProperties = entityProperties
            .Join(
                  contractProperties,
                  entityProperty => new { entityProperty.Name, entityProperty.PropertyType },
                  contractProperty => new { contractProperty.Name, contractProperty.PropertyType },
                  (entityProperty, contractProperty) => new { entityProperty, contractProperty })
            .Where(x => x.contractProperty.CanWrite);

        foreach (var match in matchingProperties)
        {
            var value = match.entityProperty.GetValue(codebookRecordEntity);
            match.contractProperty.SetValue(contract, value);
        }

        return contract;
    }

    private static List<TTenantValue> GetTenantValues<TTenantValue>(List<TCodebookRecordEntity>? codebookRecordEntities)
    where TTenantValue : class, ITenantValue, new()
    {
        if (codebookRecordEntities?.Count is null or 0)
        {
            return [];
        }

        var tenantValues = new List<TTenantValue>();
        var codebookRecordTenantValueEntities = codebookRecordEntities.SelectMany(codebookRecordEntity => codebookRecordEntity.TenantValues!.Select(tenantValueEntity => (codebookRecordEntity, tenantValueEntity)));
        foreach (var (codebookRecordEntity, tenantValueEntity) in codebookRecordTenantValueEntities)
        {
            if (codebookRecordEntity.IsDefault && !tenantValueEntity.IsForbidden)
            {
                continue;
            }

            var tenantValue = new TTenantValue();
            CommonObjectReflectionHelper.MapProperties(tenantValueEntity, tenantValue);
            CommonObjectReflectionHelper.MapProperties(codebookRecordEntity, tenantValue);
            tenantValues.Add(tenantValue);
        }

        return tenantValues;
    }

    private async Task AddDefaultTranslationAsync(string stringValue, string defaultTranslation)
    {
        var defaultTranslationEntity = await languageRepository.GetAsync(LanguageConstants.DefaultLanguageId);

        var translationEntity = new TranslationEntity
        {
            TableName = tableName,
            TableStringValue = stringValue,
            Value = defaultTranslation,
            LanguageId = LanguageConstants.DefaultLanguageId,
            Language = defaultTranslationEntity,
        };

        await translationRepository.AddAsync(translationEntity);
        translationRepository.UnitOfWork.Commit();
    }

    private async Task UpdateDefaultTranslationAsync(string stringValue, string defaultTranslation)
    {
        var defaultTranslationEntity = await translationRepository.GetTranslationAsync(stringValue, LanguageConstants.DefaultLanguageId, tableName);

        if (defaultTranslationEntity is null)
        {
            await AddDefaultTranslationAsync(stringValue, defaultTranslation);
            return;
        }

        if (defaultTranslationEntity.Value != defaultTranslation)
        {
            defaultTranslationEntity.Value = defaultTranslation;
            await translationRepository.UpdateAsync(defaultTranslationEntity);
        }
    }

    private async Task AddGlobalStringValueAsync<TAdditionalProperties, TTenantValue>(StringValueDetail<TAdditionalProperties, TTenantValue> stringValueDetail)
        where TAdditionalProperties : class, new()
        where TTenantValue : class, ITenantValue, new()
    {
        var defaultCodebookRecordEntity = MapStringValueDetailPropertiesToEntity(stringValueDetail);
        defaultCodebookRecordEntity.IsDefault = true;

        await codebookRecordRepository.AddAsync(defaultCodebookRecordEntity);
        codebookRecordRepository.UnitOfWork.Commit();

        await AddGlobalTenantValuesAsync(stringValueDetail, defaultCodebookRecordEntity);
    }

    private async Task AddLocalStringValueAsync<TAdditionalProperties, TTenantValue>(StringValueDetail<TAdditionalProperties, TTenantValue> stringValueDetail)
        where TAdditionalProperties : class, new()
        where TTenantValue : class, ITenantValue, new()
    {
        var tenantValue = stringValueDetail.TenantValues!.First();
        var defaultCodebookRecordEntity = MapStringValueDetailPropertiesToEntity(stringValueDetail);

        CommonObjectReflectionHelper.MapProperties(tenantValue, defaultCodebookRecordEntity);

        await codebookRecordRepository.AddAsync(defaultCodebookRecordEntity);
        codebookRecordRepository.UnitOfWork.Commit();

        await AddTenantValuesAsync([tenantValue.TenantId], defaultCodebookRecordEntity.Id, !IsForbidden);
    }

    private async Task AddGlobalTenantValuesAsync<TAdditionalProperties, TTenantValue>(StringValueDetail<TAdditionalProperties, TTenantValue> stringValueDetail, TCodebookRecordEntity defaultCodebookRecordEntity)
        where TAdditionalProperties : class, new()
        where TTenantValue : class, ITenantValue, new()
    {
        await AddTenantValuesAsync(stringValueDetail.TenantValues?.Where(e => e.IsForbidden).Select(tv => tv.TenantId) ?? [], defaultCodebookRecordEntity.Id, IsForbidden);

        var allActiveTenantIds = await tenantRepository.GetAllTenantIdsAsync();
        var userDefinedTenantValueTenantIds = stringValueDetail.TenantValues?.Select(e => e.TenantId) ?? [];
        var remainingTenantIds = allActiveTenantIds.Except(userDefinedTenantValueTenantIds);
        await AddTenantValuesAsync(remainingTenantIds, defaultCodebookRecordEntity.Id, !IsForbidden);

        await AddTenantValuesWithAdditionalPropertiesAsync(stringValueDetail);
    }

    private async Task AddTenantValuesWithAdditionalPropertiesAsync<TAdditionalProperties, TTenantValue>(StringValueDetail<TAdditionalProperties, TTenantValue> stringValueDetail)
            where TAdditionalProperties : class, new()
            where TTenantValue : class, ITenantValue, new()
    {
        foreach (var tenantValue in stringValueDetail.TenantValues?.Where(tenantValue => !tenantValue.IsForbidden && ObjectReflectionHelper.ContainsAdditionalTenantProperties(tenantValue)) ?? [])
        {
            var codebookRecordEntity = MapStringValueDetailPropertiesToEntity(stringValueDetail);
            CommonObjectReflectionHelper.MapProperties(tenantValue, codebookRecordEntity);
            await codebookRecordRepository.AddAsync(codebookRecordEntity);
            codebookRecordRepository.UnitOfWork.Commit();

            var tenantValueEntity = new TenantValueEntity
            {
                TenantId = tenantValue.TenantId,
                IsForbidden = tenantValue.IsForbidden,
                TableName = tableName,
                CodebookRecordId = codebookRecordEntity.Id,
            };

            await tenantValueRepository.AddAsync(tenantValueEntity);
            tenantValueRepository.UnitOfWork.Commit();
        }
    }

    private async Task UpdateTenantValuesNotToBeForbiddenForNonDefaultRecords<TAdditionalProperties, TTenantValue>(StringValueDetailEditModel<TAdditionalProperties, TTenantValue> stringValueDetail, List<TCodebookRecordEntity> codebookRecordEntities)
        where TAdditionalProperties : class, new()
        where TTenantValue : class, ITenantValue, new()
    {
        var nonDefaultTenantValues = stringValueDetail.TenantValues?.Where(e => !e.IsForbidden).ToList() ?? [];

        foreach (var nonDefaultTenantValueModel in nonDefaultTenantValues)
        {
            var codebookRecordEntity = codebookRecordEntities.First(e => e.TenantValues!.Any(tv => tv.TenantId == nonDefaultTenantValueModel.TenantId));
            var tenantValueEntity = codebookRecordEntity.TenantValues!.First(e => e.TenantId == nonDefaultTenantValueModel.TenantId);
            if (codebookRecordEntity.IsDefault)
            {
                var newCodebookRecordEntity = MapStringValueDetailPropertiesToEntity(stringValueDetail);
                newCodebookRecordEntity.StringValue = codebookRecordEntity.StringValue;
                CommonObjectReflectionHelper.MapProperties(nonDefaultTenantValueModel, newCodebookRecordEntity);
                await codebookRecordRepository.AddAsync(newCodebookRecordEntity);
                codebookRecordRepository.UnitOfWork.Commit();

                tenantValueEntity.IsForbidden = false;
                tenantValueEntity.CodebookRecordId = newCodebookRecordEntity.Id;

                await tenantValueRepository.UpdateAsync(tenantValueEntity);
            }
            else
            {
                codebookRecordEntity.Description = stringValueDetail.Description;
                CommonObjectReflectionHelper.MapProperties(nonDefaultTenantValueModel, codebookRecordEntity);
                await codebookRecordRepository.UpdateAsync(codebookRecordEntity);

                tenantValueEntity.IsForbidden = false;
                await tenantValueRepository.UpdateAsync(tenantValueEntity);
            }
        }
    }

    private async Task UpdateTenantValuesNotToBeForbiddenForDefaultRecord<TAdditionalProperties, TTenantValue>(List<TTenantValue>? tenantValues, List<TCodebookRecordEntity> codebookRecordEntities, int defaultCodebookRecordEntityId)
        where TAdditionalProperties : class, new()
        where TTenantValue : class, ITenantValue, new()
    {
        var allActiveTenantIds = await tenantRepository.GetAllTenantIdsAsync();

        var tenantIdsNotToBeForbidden = allActiveTenantIds.Except(tenantValues?.Select(x => x.TenantId) ?? []);
        var tenantValueEntitiesNotToBeForbidden = codebookRecordEntities.SelectMany(e => e.TenantValues!).Where(e => tenantIdsNotToBeForbidden.Contains(e.TenantId));

        foreach (var tenantValueEntity in tenantValueEntitiesNotToBeForbidden)
        {
            if (tenantValueEntity.IsForbidden && tenantValueEntity.CodebookRecordId == defaultCodebookRecordEntityId)
            {
                tenantValueEntity.IsForbidden = false;
                await tenantValueRepository.UpdateAsync(tenantValueEntity);
            }
            else if (tenantValueEntity.CodebookRecordId != defaultCodebookRecordEntityId)
            {
                await codebookRecordRepository.RemoveAsync(codebookRecordEntities.First(e => e.Id == tenantValueEntity.CodebookRecordId));

                tenantValueEntity.CodebookRecordId = defaultCodebookRecordEntityId;
                tenantValueEntity.IsForbidden = false;

                await tenantValueRepository.UpdateAsync(tenantValueEntity);
            }
        }
    }

    private async Task UpdateTenantValuesToBeForbidden<TAdditionalProperties, TTenantValue>(List<TTenantValue>? tenantValues, List<TCodebookRecordEntity> codebookRecordEntities)
        where TAdditionalProperties : class, new()
        where TTenantValue : class, ITenantValue, new()
    {
        var tenantIdsToBeForbidden = tenantValues?.Where(e => e.IsForbidden).Select(e => e.TenantId).ToList() ?? [];
        var tenantValueEntitiesToForbid = codebookRecordEntities.SelectMany(e => e.TenantValues!).Where(e => !e.IsForbidden && tenantIdsToBeForbidden.Contains(e.TenantId)).ToList();
        foreach (var tenantValueEntity in tenantValueEntitiesToForbid)
        {
            tenantValueEntity.IsForbidden = true;
            await tenantValueRepository.UpdateAsync(tenantValueEntity);
        }
    }

    private async Task UpdateDefaultCodebookRecord<TAdditionalProperties, TTenantValue>(StringValueDetailEditModel<TAdditionalProperties, TTenantValue> stringValueDetail, TCodebookRecordEntity defaultCodebookRecordEntity)
        where TAdditionalProperties : class, new()
        where TTenantValue : class, ITenantValue, new()
    {
        MapStringValueDetailPropertiesToEntity(stringValueDetail, defaultCodebookRecordEntity);
        await codebookRecordRepository.UpdateAsync(defaultCodebookRecordEntity);
    }

    private async Task<StringValueDetail<TAdditionalProperties, TTenantValue>> CreateResponseAsync<TAdditionalProperties, TTenantValue>(
        List<TCodebookRecordEntity> codebookRecordEntities)
    where TAdditionalProperties : class, new()
    where TTenantValue : class, ITenantValue, new()
    {
        var defaultTranslation = await translationRepository.GetTranslationAsync(codebookRecordEntities[0].StringValue, LanguageConstants.DefaultLanguageId, tableName);
        var response = new StringValueDetail<TAdditionalProperties, TTenantValue>()
        {
            StringValue = codebookRecordEntities[0].StringValue,
            Description = codebookRecordEntities[0].Description,
            IsGlobal = codebookRecordEntities[0].IsGlobal,
            DefaultAdditionalProperties = GetDefaultAdditionalProperties<TAdditionalProperties>(codebookRecordEntities.FirstOrDefault(x => x.IsDefault)),
            TenantValues = GetTenantValues<TTenantValue>(codebookRecordEntities),
            DefaultTranslation = defaultTranslation?.Value ?? string.Empty,
        };

        return response;
    }

    private async Task AddTenantValuesAsync(IEnumerable<int> tenantIds, int codebookRecordEntityId, bool isForbidden)
    {
        foreach (var tenantId in tenantIds)
        {
            var tenantValueEntity = new TenantValueEntity
            {
                TenantId = tenantId,
                TableName = tableName,
                IsForbidden = isForbidden,
                CodebookRecordId = codebookRecordEntityId,
            };
            await tenantValueRepository.AddAsync(tenantValueEntity);
        }

        tenantValueRepository.UnitOfWork.Commit();
    }

    private async Task<List<StringValueDetail<TAdditionalProperties, TTenantValue>>> CreateResponseListAsync<TAdditionalProperties, TTenantValue>(
    IEnumerable<IGrouping<string, TCodebookRecordEntity>> groupedCodebookRecordEntities)
        where TAdditionalProperties : class, new()
        where TTenantValue : class, ITenantValue, new()
    {
        var responseList = new List<StringValueDetail<TAdditionalProperties, TTenantValue>>();

        foreach (var groupedCodebookRecordEntity in groupedCodebookRecordEntities)
        {
            var response = await CreateResponseAsync<TAdditionalProperties, TTenantValue>([.. groupedCodebookRecordEntity]);
            responseList.Add(response);
        }

        return responseList;
    }
}