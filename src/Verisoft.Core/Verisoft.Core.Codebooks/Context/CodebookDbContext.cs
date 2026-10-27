using Microsoft.EntityFrameworkCore;
using Verisoft.Core.Codebooks.Common.Entities;
using Verisoft.Core.Codebooks.Common.Entities.Configurations;
using Verisoft.Core.Codebooks.Common.InstallExtensions.CodebookRegistration;
using Verisoft.Core.Common.Configuration;
using Verisoft.Core.Data.EntityFramework.Extensions;

namespace Verisoft.Core.Codebooks.Context;

public class CodebookDbContext(
    IDatabaseConfig databaseConfig,
    ICodebookRegistry codebookRegistry)
    : DbContext, ICodebookDbContext
{
    public DbSet<LanguageEntity> Language { get; set; }

    public DbSet<CodebookEntity> Codebook { get; set; }

    public void Commit()
    {
        SaveChanges();
    }

    public Task CommitAsync(CancellationToken cancellationToken = default)
    {
        return SaveChangesAsync(cancellationToken);
    }

    public void Migrate()
    {
        Database.Migrate();
    }

    public void Rollback()
    {
        this.RollbackChanges();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var configurationAssembly = typeof(CodebookBaseEntityTypeConfiguration<>).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(configurationAssembly);

        modelBuilder.ApplyCodebookRecordConfigurations(codebookRegistry);
        modelBuilder.HasDefaultSchema(databaseConfig.DefaultSchema);
    }
}