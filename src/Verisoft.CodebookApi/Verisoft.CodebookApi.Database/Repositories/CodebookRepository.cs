using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Verisoft.CodebookApi.Core.Entities;
using Verisoft.CodebookApi.Core.Repositories;
using Verisoft.CodebookApi.Database.Context;
using Verisoft.Core.Authentication;
using Verisoft.Core.Data.EntityFramework.Repositories;

namespace Verisoft.CodebookApi.Database.Repositories;

public class CodebookRepository(ICodebookApiDbContext unitOfWork, IUserContext userContext, ILogger<CodebookRepository> logger) : EntityFrameworkRepositoryBase<CodebookEntity, int>(unitOfWork, userContext, logger), ICodebookRepository
{
    protected ICodebookApiDbContext Context => (ICodebookApiDbContext)UnitOfWork;

    public async Task<CodebookEntity?> GetCodebookByName(string name)
    {
        return await GetDbSet().FirstOrDefaultAsync(c => c.Name == name);
    }

    protected override DbSet<CodebookEntity> GetDbSet()
    {
        return Context.Codebook;
    }
}
