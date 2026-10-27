using Microsoft.EntityFrameworkCore;
using Verisoft.Core.Codebooks.Common.Entities;
using Verisoft.Core.Codebooks.Common.LanguageServices;
using Verisoft.Core.Codebooks.Context;

namespace Verisoft.Core.Codebooks.Repositories;

public class LanguageRepository(
    ICodebookDbContext unitOfWork,
    ILanguageProvider languageProvider)
    : CodebookBaseRepository<LanguageEntity, int>(unitOfWork, languageProvider),
    ILanguageRepository
{
    protected ICodebookDbContext Context => (ICodebookDbContext)UnitOfWork;

    public Task<LanguageEntity?> GetByCodeAsync(string code)
    {
        return GetQueryable().FirstOrDefaultAsync(x => x.Code == code);
    }

    protected override DbSet<LanguageEntity> GetDbSet()
    {
        return Context.Language;
    }
}
