using Verisoft.Core.Codebooks.Common.Entities;

namespace Verisoft.Core.Codebooks.Repositories;

public interface ICodebookRepository : ICodebookBaseRepository<CodebookEntity, int>
{
    Task<CodebookEntity?> GetCodebookByName(string name);
}
