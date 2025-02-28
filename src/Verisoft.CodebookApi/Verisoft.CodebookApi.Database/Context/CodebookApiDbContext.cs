using Microsoft.EntityFrameworkCore;
using Verisoft.CodebookApi.Core.Entities;
using Verisoft.CodebookApi.Database.Configurations;
using Verisoft.Core.Common.Configuration;
using Verisoft.Core.Data.EntityFramework;

namespace Verisoft.CodebookApi.Database.Context;

public class CodebookApiDbContext(IDatabaseConfig databaseConfig, ICodebookRecordSchemaManager codebookRecordSchemaManager) : EntityFrameworkDbContextBase, ICodebookApiDbContext
{
    private readonly IDatabaseConfig databaseConfig = databaseConfig ?? throw new ArgumentNullException(nameof(databaseConfig));

    public DbSet<LanguageEntity> Language { get; set; }

    public DbSet<CodebookEntity> Codebook { get; set; }

    public DbSet<TranslationEntity> Translation { get; set; }

    public DbSet<TenantValueEntity> TenantValue { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
                databaseConfig.ConnectionString,
                x => x.MigrationsHistoryTable("__MigrationsHistory", databaseConfig.DefaultSchema));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CodebookApiDbContext).Assembly);

        codebookRecordSchemaManager.ConfigureTables(modelBuilder);
    }
}
