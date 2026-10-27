using Verisoft.Core.Codebooks.Common.Models;
using Verisoft.Core.Common.Entities.Codebooks;
using Verisoft.Core.Common.Store;

namespace Verisoft.Core.Codebooks.Repositories;

public interface ICodebookRecordRepository<TCodebookRecordType> : IRepository<CodebookRecordEntity<TCodebookRecordType>, int>
    where TCodebookRecordType : CodebookRecordType, new()
{
    Task<CodebookRecordEntity<TCodebookRecordType>?> GetStringValueDetailAsync(string stringValue);

    Task<int> GetCodebookRecordsByTranslationCountAsync(string languageCode, string? translationFilter, string[]? stringValue = null, string[]? exclude = null);

    Task<CodebookRecordWithTranslation<TCodebookRecordType>[]> GetPagedCodebookRecordsByTranslationAsync(int offset, int limit, string languageCode, string? translationFilter, bool isAscending = true, string[]? stringValue = null, string[]? exclude = null);

    void HardDeleteCodebookRecord(CodebookRecordEntity<TCodebookRecordType> record);
}