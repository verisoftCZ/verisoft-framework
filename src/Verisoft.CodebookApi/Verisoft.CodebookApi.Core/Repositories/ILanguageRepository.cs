using Verisoft.CodebookApi.Core.Entities;
using Verisoft.Core.Common.Store;

namespace Verisoft.CodebookApi.Core.Repositories;

public interface ILanguageRepository : IRepository<LanguageEntity, int>
{
    Task<LanguageEntity?> GetByCodeAsync(string code);
}
