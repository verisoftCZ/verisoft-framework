using Microsoft.EntityFrameworkCore;
using Verisoft.Core.Codebooks.Common.Entities;
using Verisoft.Core.Codebooks.Common.LanguageServices;
using Verisoft.Core.Codebooks.Context;

namespace Verisoft.Core.Codebooks.Repositories;

public class CodebookRepository(ICodebookDbContext unitOfWork, ILanguageProvider languageProvider) : CodebookBaseRepository<CodebookEntity, int>(unitOfWork, languageProvider), ICodebookRepository
{
    protected ICodebookDbContext Context => (ICodebookDbContext)UnitOfWork;

    public async Task<CodebookEntity?> GetCodebookByName(string name)
    {
        return await GetDbSet().FirstOrDefaultAsync(c => c.Name == name);
    }

    protected override DbSet<CodebookEntity> GetDbSet()
    {
        return Context.Codebook;
    }
}
