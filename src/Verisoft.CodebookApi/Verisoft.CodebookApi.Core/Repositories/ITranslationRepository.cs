using Verisoft.CodebookApi.Core.Entities;
using Verisoft.Core.Common.Store;

namespace Verisoft.CodebookApi.Core.Repositories;

public interface ITranslationRepository : IRepository<TranslationEntity, int>
{
    Task<TranslationEntity?> GetTranslationAsync(string stringValue, int languageId, string tableName);

    Task<List<TranslationEntity>> GetTranslationsAsync(string stringValue, string tableName);
}
