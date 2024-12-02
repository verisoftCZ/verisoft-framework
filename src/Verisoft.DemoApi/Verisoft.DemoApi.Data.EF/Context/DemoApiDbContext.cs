using Microsoft.EntityFrameworkCore;
using Verisoft.Core.Common.Configuration;
using Verisoft.Core.Data.EntityFramework;
using Verisoft.DemoApi.Common.Entities;

namespace Verisoft.DemoApi.Data.EF.Context;

public class DemoApiDbContext(IDatabaseConfig databaseConfig) : EntityFrameworkDbContextBase, IDemoApiDbContext
{
    private readonly IDatabaseConfig databaseConfig = databaseConfig ?? throw new ArgumentNullException(nameof(databaseConfig));

    public DbSet<ClientEntity> Client { get; set; }

    public DbSet<UserEntity> User { get; set; }

    public DbSet<DocumentEntity> Document { get; set; }

    public DbSet<BlobEntity> Blob { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DemoApiDbContext).Assembly);
        modelBuilder.HasDefaultSchema(databaseConfig.DefaultSchema);

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