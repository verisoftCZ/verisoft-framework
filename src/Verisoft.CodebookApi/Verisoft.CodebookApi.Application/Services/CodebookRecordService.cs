using Verisoft.CodebookApi.Application.Services.Interfaces;
using Verisoft.CodebookApi.Contracts.Filters;
using Verisoft.CodebookApi.Contracts.Models.Base;
using Verisoft.CodebookApi.Contracts.Models.CodebookRecord;
using Verisoft.CodebookApi.Core.Constants;
using Verisoft.CodebookApi.Core.Entities.BaseEntity;
using Verisoft.CodebookApi.Core.Repositories;
using Verisoft.Core.Authentication;
using Verisoft.Core.Contracts;
using Verisoft.Core.Contracts.BusinessResult;
using CommonObjectReflectionHelper = Verisoft.Core.Common.Helpers.ObjectReflectionHelper;

namespace Verisoft.CodebookApi.Application.Services;

public class CodebookRecordService<TCodebookRecordEntity>(
    ICodebookRecordRepository<TCodebookRecordEntity> codebookRecordRepository,
    ITenantRepository tenantRepository,
    ILanguageRepository languageRepository,
    ITranslationRepository translationRepository,
    IUserContext userContext)
    : ICodebookRecordService<TCodebookRecordEntity>
    where TCodebookRecordEntity : class, ICodebookRecordEntity, new()
{
    private readonly string tableName = typeof(TCodebookRecordEntity).Name.Replace("Entity", string.Empty);
    private readonly ICodebookRecordRepository<TCodebookRecordEntity> codebookRecordRepository = codebookRecordRepository ?? throw new ArgumentNullException(nameof(codebookRecordRepository));
    private readonly ITenantRepository tenantRepository = tenantRepository ?? throw new ArgumentNullException(nameof(tenantRepository));
    private readonly ILanguageRepository languageRepository = languageRepository ?? throw new ArgumentNullException(nameof(languageRepository));
    private readonly ITranslationRepository translationRepository = translationRepository ?? throw new ArgumentNullException(nameof(translationRepository));
    private readonly IUserContext userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));

    public async Task<BusinessActionResult<CodebookRecordListData<TAdditionalProperties>>> GetCodebookRecordsAsync<TAdditionalProperties, TTenantValue>(
        FilteredRequest<CodebookRecordFilter> request)
        where TAdditionalProperties : class, new()
        where TTenantValue : class, ITenantValue, new()
    {
        {
            var tenantId = await GetUserTenantIdAsync();

            if (!tenantId.HasValue)
            {
                return ErrorFactory.Forbidden(userContext.User.GetUserName());
            }

            var codebookRecords = await GetCodebookRecordsByFilter(request.Filter, tenantId.Value);

            var pagedData = codebookRecords.OrderBy(x => x.Translations!.First().Value).Skip(request.Offset).Take(request.Limit);

            var simpleRecords = codebookRecords.Select(ConvertToCodebookRecordContract<TAdditionalProperties>);

            return new CodebookRecordListData<TAdditionalProperties>()
            {
                Data = simpleRecords,
                Total = codebookRecords.Count,
            };
        }
    }

    public async Task<BusinessActionResult<CodebookRecord<TAdditionalProperties>>> GetCodebookRecordAsync<TAdditionalProperties, TTenantValue>(
        string stringValue,
        string? languageCode)
        where TAdditionalProperties : class, new()
        where TTenantValue : class, ITenantValue, new()
    {
        {
            var tenantId = await GetUserTenantIdAsync();

            if (!tenantId.HasValue)
            {
                return ErrorFactory.Forbidden(userContext.User.GetUserName());
            }

            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return ErrorFactory.BadRequest("StringValue is required");
            }

            var codebookRecords = await codebookRecordRepository.GetCodebookRecordsAsync(stringValue);
            var codebookRecord = codebookRecords.FirstOrDefault(x => x.TenantValues!.Any(tv => tv.TenantId == tenantId));
            if (codebookRecord is null)
            {
                return ErrorFactory.NotFound<TCodebookRecordEntity>("stringValue", stringValue);
            }

            var translations = await translationRepository.GetTranslationsAsync(stringValue, tableName);

            var language = await languageRepository.GetByCodeAsync(languageCode ?? LanguageConstants.DefaultLanguageCode);
            codebookRecord.Translations = [translations.FirstOrDefault(x => x.LanguageId == language?.Id) ?? translations.First(x => x.LanguageId == LanguageConstants.DefaultLanguageId)];

            return ConvertToCodebookRecordContract<TAdditionalProperties>(codebookRecord);
        }
    }

    public async Task<BusinessActionResult<bool>> IsCodebookRecordAvailableAsync<TAdditionalProperties, TTenantValue>(string stringValue)
        where TAdditionalProperties : class, new()
        where TTenantValue : class, ITenantValue, new()
    {
        var tenantId = await GetUserTenantIdAsync();

        if (!tenantId.HasValue)
        {
            return ErrorFactory.Forbidden(userContext.User.GetUserName());
        }

        var codebookRecords = await codebookRecordRepository.GetCodebookRecordsAsync(stringValue);
        var codebookRecord = codebookRecords.FirstOrDefault(x => x.TenantValues!.Any(tv => tv.TenantId == tenantId && !tv.IsForbidden));

        return codebookRecord is not null;
    }

    private static CodebookRecord<TAdditionalProperties> ConvertToCodebookRecordContract<TAdditionalProperties>(TCodebookRecordEntity codebookRecord)
        where TAdditionalProperties : class, new()
    {
        var additionalProperties = new TAdditionalProperties();
        CommonObjectReflectionHelper.MapProperties(codebookRecord, additionalProperties);
        return new CodebookRecord<TAdditionalProperties>
        {
            StringValue = codebookRecord.StringValue,
            IsForbidden = codebookRecord.TenantValues!.First().IsForbidden,
            Translation = codebookRecord.Translations!.First().Value,
            AdditionalProperties = additionalProperties,
        };
    }

    private async Task<List<TCodebookRecordEntity>> GetCodebookRecordsByFilter(CodebookRecordFilter filter, int tenantId)
    {
        var language = await languageRepository.GetByCodeAsync(filter.LanguageCode ?? LanguageConstants.DefaultLanguageCode);

        var codebookRecords = await codebookRecordRepository.GetCodebookRecordsByTranslationAsync(
            filter.Translation,
            tenantId,
            language?.Id ?? LanguageConstants.DefaultLanguageId);

        if (filter.StringValueArray?.Length > 0)
        {
            codebookRecords = codebookRecords.Where(x => filter.StringValueArray.Contains(x.StringValue)).ToList();
        }

        if (filter.StringValueArrayToExclude?.Length > 0)
        {
            codebookRecords = codebookRecords.Where(x => !filter.StringValueArrayToExclude.Contains(x.StringValue)).ToList();
        }

        return codebookRecords;
    }

    private async Task<int?> GetUserTenantIdAsync()
    {
        return tenantRepository.IsTenantEnvironment ? throw new NotImplementedException() : (await tenantRepository.GetAllTenantIdsAsync()).First();
    }
}