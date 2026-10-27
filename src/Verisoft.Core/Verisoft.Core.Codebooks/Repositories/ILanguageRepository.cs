using Verisoft.Core.Codebooks.Common.Entities;

namespace Verisoft.Core.Codebooks.Repositories;

public interface ILanguageRepository : ICodebookBaseRepository<LanguageEntity, int>
{
    Task<LanguageEntity?> GetByCodeAsync(string code);
}
