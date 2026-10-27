#Základní info
- Entity Framework Core
- Repository pattern
- Filtrování, řazení a stránkování přes `FilteredRequest`, Specification a `BaseSort`
- Konfigurace přes IEntityTypeConfiguration a EntityTypeBuilder
- EF Migrace
- Automatický soft delete a auditní pole (viz níže)

#Soft delete, audit a interceptory (EntityFrameworkDbContextBase)

Všechny konzumující `DbContext` třídy dědí od
`Verisoft.Core.Data.EntityFramework.EntityFrameworkDbContextBase`
(`Verisoft.Core.Data.EF`), která automaticky pro **všechny entity implementující
`IBaseEntity`** (tedy entity dědící z `BaseEntity<TKey>`, viz
[Nastavení entit pro aplikaci](Databaze/Nastavení-entit-pro-aplikaci.md)) řeší:

- **Soft delete** – `SaveChanges`/`SaveChangesAsync` před uložením převede
  entity ve stavu `Deleted` na `Modified` a nastaví `IsDeleted = true`
  (`ProcessSoftDeletesAsync`). Smazání se navíc kaskádovitě propaguje na
  navázané entity pomocí `CascadeWalker.WalkAndSoftDeleteAsync`.
- **Globální query filter** – `OnModelCreating` automaticky přidá pro každou
  entitu implementující `IBaseEntity` filtr `!IsDeleted`
  (`AddSoftDeleteQueryFilterToBaseEntities`), takže smazané záznamy se
  standardně nevrací v žádném dotazu. **Není potřeba přidávat vlastní
  `HasQueryFilter(x => !x.IsDeleted)` do `IEntityTypeConfiguration` –
  base třída to dělá globálně za všechny entity** (viz
  [Db konfigurace](Databaze/Db-konfigurace.md)).
- **Auditní pole** – `CreatedBy`/`CreatedAt` se nastaví při vložení a
  `UpdatedBy`/`UpdatedAt` při úpravě nebo smazání (`ProcessAuditFields`) – toto
  se děje automaticky v `SaveChanges`/`SaveChangesAsync`, není potřeba volat
  žádnou další metodu na entitě. Pozn.: framework dříve obsahoval i
  podrobnější audit historie hodnot jednotlivých property (samostatné
  tabulky `EntityAudit`/`EntityAuditDetail`) – ta byla odstraněna, viz
  [Audit](Databaze/Audit.md).
- **Optimistic concurrency** – entity implementující `IOptimisticLockable`
  automaticky získají EF `Version` property nakonfigurovanou jako
  `IsRowVersion()` (`AddVersionPropertyForOptimisticConcurrency`). Samotné
  vyhodnocení konfliktu (`OriginalValue` vs. aktuální hodnota) i explicitní
  pesimistické zamykání záznamů (`ILockingManager`) řeší modul
  `Verisoft.Core.AspNet.Concurency`, viz
  [Concurrency (zamykání záznamů)](Concurrency.md).
- **Automatická registrace EF interceptorů** – veřejné třídy implementující
  `IInterceptor` (např. `ISaveChangesInterceptor`) označené atributem
  `[RegisterInterceptor]` (`Verisoft.Core.Data.EntityFramework.Atributes`) se
  automaticky posbírají ze všech načtených assemblies s prefixem `Verisoft` a
  zaregistrují do `DbContextOptionsBuilder` (`RegisterInterceptorsByAttribute`
  v `OnConfiguring`). Není tedy potřeba interceptory registrovat ručně v
  `InstallExtensions` – stačí třídu opatřit atributem. Příklad:
  `Verisoft.Core.AspNet.Concurency.DataInterceptors.OptimisticConcurrencyInterceptor`.

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
