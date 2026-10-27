using Verisoft.Core.Codebooks.Common.Filters;
using Verisoft.Core.Codebooks.Common.Models;
using Verisoft.Core.Codebooks.Constants;
using Verisoft.Core.Codebooks.Repositories;
using Verisoft.Core.Common.Entities.Codebooks;
using Verisoft.Core.Contracts;
using Verisoft.Core.Contracts.BusinessResult;

namespace Verisoft.Core.Codebooks.Services;

public class CodebookRecordService<TCodebookRecordType>(
    ICodebookRecordRepository<TCodebookRecordType> codebookRecordRepository)
    : ICodebookRecordService<TCodebookRecordType>
    where TCodebookRecordType : CodebookRecordType, new()
{
    private readonly ICodebookRecordRepository<TCodebookRecordType> codebookRecordRepository = codebookRecordRepository ?? throw new ArgumentNullException(nameof(codebookRecordRepository));

    public virtual async Task<BusinessActionResult<CodebookRecordListData<TCodebookRecordType>>> GetCodebookRecordsAsync(
        FilteredRequest<CodebookRecordFilter> request)
    {
        var count = await codebookRecordRepository.GetCodebookRecordsByTranslationCountAsync(
            request.Filter?.LanguageCode ?? LanguageConstants.DefaultLanguageCode,
            request.Filter?.Translation,
            request.Filter?.StringValue,
            request.Filter?.Exclude);

        var sorts = request.ParsedSorts ?? Enumerable.Empty<SortDefinition>();
        var isAscending = sorts.FirstOrDefault()?.Direction == SortDirection.Asc;

        var records = await codebookRecordRepository.GetPagedCodebookRecordsByTranslationAsync(
            request.Offset,
            request.Limit,
            request.Filter?.LanguageCode ?? LanguageConstants.DefaultLanguageCode,
            request.Filter?.Translation,
            isAscending,
            request.Filter?.StringValue,
            request.Filter?.Exclude);

        return new CodebookRecordListData<TCodebookRecordType>()
        {
            Data = records.Select(ConvertToCodebookRecordContract),
            Total = count,
        };
    }

    public virtual async Task<BusinessActionResult<CodebookRecord<TCodebookRecordType>>> GetCodebookRecordAsync(
        string stringValue,
        string? languageCode)
    {
        if (string.IsNullOrWhiteSpace(stringValue))
        {
            return ErrorFactory.BadRequest("StringValue is required");
        }

        var codebookRecord = await codebookRecordRepository.GetStringValueDetailAsync(stringValue);
        if (codebookRecord is null)
        {
            return ErrorFactory.NotFound<CodebookRecordEntity<TCodebookRecordType>>(nameof(CodebookRecordEntity<TCodebookRecordType>.StringValue), stringValue);
        }

        var codebookRecordWithTranslation = new CodebookRecordWithTranslation<TCodebookRecordType>
        {
            Record = codebookRecord,
            Translation = GetTranslationValueOrDefault(codebookRecord, languageCode),
        };

        return ConvertToCodebookRecordContract(codebookRecordWithTranslation);
    }

    public virtual async Task<BusinessActionResult<bool>> IsCodebookRecordAvailableAsync(string stringValue)
    {
        var codebookRecord = await codebookRecordRepository.GetStringValueDetailAsync(stringValue);
        return codebookRecord?.IsValid is true;
    }

    protected virtual CodebookRecord<TCodebookRecordType> ConvertToCodebookRecordContract(CodebookRecordWithTranslation<TCodebookRecordType> codebookRecord)
    {
        return new CodebookRecord<TCodebookRecordType>
        {
            StringValue = codebookRecord.Record.StringValue,
            IsValid = codebookRecord.Record.IsValid,
            Description = codebookRecord.Record.Description,
            Translation = codebookRecord.Translation,
            AdditionalProperties = codebookRecord.Record.AdditionalProperties,
        };
    }

    private string GetTranslationValueOrDefault(CodebookRecordEntity<TCodebookRecordType> record, string? languageCode)
    {
        return record.Translations
            .FirstOrDefault(x => x.LanguageCode == languageCode)?.Value
            ??
            record.Translations
            .First(x => x.LanguageCode == LanguageConstants.DefaultLanguageCode).Value;
    }
}