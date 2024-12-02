#Základní info
- Entity Framework
- Repository pattern
- Konfigurace přes IEntityTypeConfiguration a EntityTypeBuilder
- EF Migrace
- Soft delete (bool IsDeleted)

#Setup
**DbContext**

Interface, který dědí od IUnitOfWork a IMigrable

``` csharp
public interface IDemoApiDbContext : IUnitOfWork, IMigrable
{
    public DbSet<ClientEntity> Client { get; set; }
}
```

IUnitOfWork umožňuje Commit a Rollback
IMigrable zase Migrate

Třída, která dědí od EntityFrameworkDbContextBase


``` csharp
public class DemoApiDbContext(IDatabaseConfig databaseConfig) : EntityFrameworkDbContextBase, IDemoApiDbContext
{
    private readonly IDatabaseConfig databaseConfig = databaseConfig ?? throw new ArgumentNullException(nameof(databaseConfig));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DemoApiDbContext).Assembly);
        modelBuilder.HasDefaultSchema(databaseConfig.DefaultSchema);
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
```

Registrace        
``` csharp
serviceCollection.TryAddSingleton<IDatabaseConfig, DatabaseConfig>();
serviceCollection.TryAddScoped<IDemoApiDbContext, DemoApiDbContext>();
```
