using Verisoft.CodebookApi.Core.Entities.BaseEntity;
using Verisoft.Core.Common.Store;

namespace Verisoft.CodebookApi.Core.Repositories;

public interface ICodebookRecordRepository<TCodebookRecordEntity> : IRepository<TCodebookRecordEntity, int>
    where TCodebookRecordEntity : ICodebookRecordEntity
{
    Task<List<TCodebookRecordEntity>> GetCodebookRecordsAsync(string? stringValue = null, string? searchTerm = null);

    Task<List<TCodebookRecordEntity>> GetCodebookRecordsByTranslationAsync(string? translationFilter, int tenantId, int languageId);
}
