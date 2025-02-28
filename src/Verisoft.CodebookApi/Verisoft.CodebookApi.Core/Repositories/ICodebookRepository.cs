using Verisoft.CodebookApi.Core.Entities;
using Verisoft.Core.Common.Store;

namespace Verisoft.CodebookApi.Core.Repositories;

public interface ICodebookRepository : IRepository<CodebookEntity, int>
{
    Task<CodebookEntity?> GetCodebookByName(string name);
}
