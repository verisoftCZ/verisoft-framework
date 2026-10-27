using Microsoft.EntityFrameworkCore;
using Verisoft.Core.Authentication;
using Verisoft.Core.Codebooks.Common.Entities.Configurations;
using Verisoft.Core.Codebooks.Common.InstallExtensions.CodebookRegistration;
using Verisoft.Core.Common.Configuration;
using Verisoft.Core.Data.EntityFramework;
using Verisoft.Core.Data.EntityFramework.Walkers;
using Verisoft.DemoApi.Common.Entities;
using Verisoft.DemoApi.Contracts.Sagas;

namespace Verisoft.DemoApi.Data.EF.Context;

public class DemoApiDbContext(IDatabaseConfig databaseConfig, ICodebookRegistry codebookRegistry, IUserContext userContext, IWalkerFactory walkerFactory, TimeProvider timeProvider) : EntityFrameworkDbContextBase(userContext, walkerFactory, timeProvider), IDemoApiDbContext
{
    private readonly IDatabaseConfig databaseConfig = databaseConfig ?? throw new ArgumentNullException(nameof(databaseConfig));
    private readonly ICodebookRegistry codebookRegistry = codebookRegistry ?? throw new ArgumentNullException(nameof(codebookRegistry));

    public DbSet<ClientEntity> Client { get; set; }

    public DbSet<UserEntity> User { get; set; }

    public DbSet<DocumentEntity> Document { get; set; }

    public DbSet<BlobEntity> Blob { get; set; }

    public DbSet<DocumentSagaState> DocumentSagaState { get; set; }

    public DbSet<CodebookParentEntity> CodebookParent { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DemoApiDbContext).Assembly);
        modelBuilder.HasDefaultSchema(databaseConfig.DefaultSchema);
        modelBuilder.ApplyCodebookRecordConfigurations(codebookRegistry, true);

        modelBuilder.Entity<UserEntity>()
            .HasIndex(u => u.Email)
            .IsUnique()
            .HasFilter($"[{nameof(UserEntity.IsDeleted)}] = 0");
    }

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
}