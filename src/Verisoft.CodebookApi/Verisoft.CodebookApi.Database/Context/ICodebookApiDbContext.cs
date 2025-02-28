using Microsoft.EntityFrameworkCore;
using Verisoft.CodebookApi.Core.Entities;
using Verisoft.Core.Common.Store;

namespace Verisoft.CodebookApi.Database.Context;

public interface ICodebookApiDbContext : IUnitOfWork, IMigrable
{
    public DbSet<LanguageEntity> Language { get; set; }

    public DbSet<CodebookEntity> Codebook { get; set; }

    public DbSet<TranslationEntity> Translation { get; set; }

    public DbSet<TenantValueEntity> TenantValue { get; set; }
}
