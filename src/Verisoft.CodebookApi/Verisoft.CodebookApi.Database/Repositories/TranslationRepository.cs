using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Verisoft.CodebookApi.Core.Entities;
using Verisoft.CodebookApi.Core.Repositories;
using Verisoft.CodebookApi.Database.Context;
using Verisoft.Core.Authentication;
using Verisoft.Core.Data.EntityFramework.Repositories;

namespace Verisoft.CodebookApi.Database.Repositories;

public class TranslationRepository(ICodebookApiDbContext unitOfWork, IUserContext userContext, ILogger<TranslationRepository> logger) : EntityFrameworkRepositoryBase<TranslationEntity, int>(unitOfWork, userContext, logger), ITranslationRepository
{
    protected ICodebookApiDbContext Context => (ICodebookApiDbContext)UnitOfWork;

    public async Task<TranslationEntity?> GetTranslationAsync(string stringValue, int languageId, string tableName)
    {
        return await GetQueryable().FirstOrDefaultAsync(t => t.TableStringValue == stringValue && t.LanguageId == languageId && t.TableName == tableName);
    }

    public async Task<List<TranslationEntity>> GetTranslationsAsync(string stringValue, string tableName)
    {
        return await GetQueryable().Where(t => t.TableStringValue == stringValue && t.TableName == tableName).ToListAsync();
    }

    protected override DbSet<TranslationEntity> GetDbSet()
    {
        return Context.Translation;
    }
}
