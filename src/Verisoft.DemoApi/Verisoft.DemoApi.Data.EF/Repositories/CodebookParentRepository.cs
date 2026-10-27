using Microsoft.EntityFrameworkCore;
using Verisoft.DemoApi.Common.Entities;
using Verisoft.DemoApi.Common.Repositories;
using Verisoft.DemoApi.Data.EF.Context;

namespace Verisoft.DemoApi.Data.EF.Repositories;

public class CodebookParentRepository(IDemoApiDbContext unitOfWork) : BaseRepository<CodebookParentEntity, int>(unitOfWork), ICodebookParentRepository
{
    protected override IQueryable<CodebookParentEntity> GetQueryable()
        => GetDbSet()
            .Include(x => x.Client)
            .Include(x => x.AccessibilityCodebookRecord);

    protected override IQueryable<CodebookParentEntity> GetQueryableForExport()
        => GetDbSet()
            .Include(x => x.Client)
            .Include(x => x.AccessibilityCodebookRecord);

    protected override DbSet<CodebookParentEntity> GetDbSet()
        => Context.CodebookParent;
}
