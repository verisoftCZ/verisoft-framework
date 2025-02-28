using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Verisoft.CodebookApi.Core.Entities;
using Verisoft.CodebookApi.Core.Repositories;
using Verisoft.CodebookApi.Database.Context;
using Verisoft.Core.Authentication;
using Verisoft.Core.Data.EntityFramework.Repositories;

namespace Verisoft.CodebookApi.Database.Repositories;

public class LanguageRepository(ICodebookApiDbContext unitOfWork, IUserContext userContext, ILogger<LanguageRepository> logger) : EntityFrameworkRepositoryBase<LanguageEntity, int>(unitOfWork, userContext, logger), ILanguageRepository
{
    protected ICodebookApiDbContext Context => (ICodebookApiDbContext)UnitOfWork;

    public Task<LanguageEntity?> GetByCodeAsync(string code)
    {
        return GetQueryable().FirstOrDefaultAsync(x => x.Code == code);
    }

    protected override DbSet<LanguageEntity> GetDbSet()
    {
        return Context.Language;
    }
}
