#Db konfigurace
- Nepoužíváme atributy ve třídách entit, ale EntityTypeBuilder a IEntityTypeConfiguration<Entity>
- Registrace konfigurací v metodě OnModelCreating v DbContextu => `modelBuilder.ApplyConfigurationsFromAssembly(typeof(DemoApiDbContext).Assembly);`

``` csharp
public class ClientConfiguration : IEntityTypeConfiguration<ClientEntity>
{
    public void Configure(EntityTypeBuilder<ClientEntity> builder)
    {
        builder.HasMany(client => client.Subsidiaries)
               .WithOne(client => client.ParentClient)
               .HasForeignKey(client => client.ParentClientId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.ConfigureEnumAsString(x => x.TaxDomicile);
    }
}
```
**Soft delete**
Globální query filter `!IsDeleted` pro všechny entity implementující
`IBaseEntity` přidává automaticky `EntityFrameworkDbContextBase` v
`OnModelCreating` (viz [Databáze](../Databaze.md)) – **v jednotlivých
`IEntityTypeConfiguration` třídách se `HasQueryFilter(x => !x.IsDeleted)`
nepoužívá**, protože by byl nadbytečný (EF Core navíc nepodporuje víc filtrů
na stejné entitě bez explicitního „named filter“ API na .NET 10). Samotné
mazání entity (`RemoveAsync` na repository) se také automaticky mění na
`UPDATE ... SET IsDeleted = 1` místo fyzického `DELETE` – řeší base třída
kontextu, ne konfigurace entity.

Aktuální `ClientConfiguration` v DemoApi navíc používá `builder.ToTable(t => t.IsTemporal())`
pro časové (temporal) tabulky – to je nezávislá funkce EF Core a se soft
delete filtrem nesouvisí.

Samostatný případ je filtrovaný unikátní index v `DemoApiDbContext.OnModelCreating`
(`modelBuilder.Entity<UserEntity>().HasIndex(u => u.Email).IsUnique().HasFilter(...)`)
– zajišťuje unikátnost e-mailu jen mezi nesmazanými uživateli. Nejde o
soft-delete query filtr (ten je globální a řeší ho base třída kontextu), ale
o databázový index s podmínkou; oba mechanismy lze používat současně.

> **Pozor:** globální soft-delete query filtr (`!IsDeleted`) se vztahuje jen
> na dotazy (`SELECT`), **ne na indexy**. Pokud je na entitě s `IBaseEntity`
> unikátní index bez `.HasFilter(...)`, databáze by vynucovala unikátnost i
> mezi soft-deleted řádky – po smazání záznamu by tak nešlo založit nový se
> stejnou unikátní hodnotou, dokud starý zůstává v tabulce.
>
> Toto řeší automaticky metoda `AddSoftDeleteFilterToUniqueIndexes` volaná z
> `EntityFrameworkDbContextBase.OnModelCreating` (viz
> [Databáze](../Databaze.md)) – ke všem unikátním indexům entit
> implementujících `IBaseEntity` doplní filtr `IsDeleted = 0`/`FALSE` (podle
> použitého providera, SQL Server i PostgreSQL). Pokud index už vlastní filtr
> má (jako u `UserEntity.Email` výše), podmínka na `IsDeleted` se k němu jen
> přidá (`AND`), pokud v něm ještě není. V konfiguraci jednotlivých entit tedy
> není potřeba na toto myslet ručně.

**Enumy**
S enumy v Db pracujeme jako se stringy kvůli nezávislosti na pořadí zápisu do enum a neschopnosti NSwag přenášet int reprezentaci enumu do vygenerovaného klienta.
Pro konfiguraci enumu využíváme `Verisoft.Core.Data.EntityFramework.Extensions.EntityTypeBuilderExtensions.ConfigureEnumAsString()`

**M : N názvy tabulek**
- EF je schopen vygenerovat tabulky pro m : n kardinalitu sám, bez nutnosti správy těchto vazeb v kódu.
- Název tabulky je však odvozen od názvu tříd. Tím že používáme na konci názvu třídy entity slovo "Entity" v případě vazby m : n mezi entitami "Client" (třída ClientEntity) a "City" (třída CityEntity) by vznikla tabulka "ClientEntityAddressEntity".
- Je potřeba override názvu tabulky pomocí následujícího kódu a toho lze dosáhnout s extension metodou `Verisoft.Core.Data.EntityFramework.Extensions.EntityTypeBuilderExtensions.HasManyWithMany()`
        
``` csharp
builder.HasManyWithMany(
            client => client.Cities,
            city => city.Clients,
            "CityId",
            "ClientId",
            "ClientCity");
```




#EF Migrace
EF migrace probíhá přes console script
```powershell
dotnet ef migrations add InitialMigration `
  --project src/Verisoft.DemoApi/Verisoft.DemoApi.Data.EF `
  --startup-project src/Verisoft.DemoApi/Verisoft.DemoApi.Host `
  --context DemoApiDbContext
```

Spuštění migrace při startupu zajišťuje 
`Verisoft.Core.AspNet.ApplicationBuilderExtensions.MigrateDatabase<TDBContext>(this IApplicationBuilder app)`

``` csharp
public static void UseDemoApi(this IApplicationBuilder applicationBuilder)
{
    applicationBuilder.MigrateDatabase<IDemoApiDbContext>();
}
```