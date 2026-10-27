using Verisoft.Core.Common.Entities.Codebooks;
using Verisoft.Core.Common.Store;

namespace Verisoft.Core.Codebooks.Repositories;

public interface ITranslationRepository<TCodebookRecordType> : IRepository<CodebookRecordTranslationEntity<TCodebookRecordType>, int>
    where TCodebookRecordType : CodebookRecordType, new()
{
    Task<CodebookRecordTranslationEntity<TCodebookRecordType>[]> GetTranslationsByRecordIdAsync(int codebookRecordId);

    Task<CodebookRecordTranslationEntity<TCodebookRecordType>?> GetTranslationByRecordIdAndLanguageCodeAsync(int codebookRecordId, string languageCode);
}
