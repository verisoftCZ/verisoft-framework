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

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
```
**Soft delete**
Pro zamezení získání smazaných entit je potřeba nastavit `builder.HasQueryFilter(x => !x.IsDeleted);`

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
`dotnet ef migrations add Initial_Migration --project C:\\...\Verisoft.DemoApi\Verisoft.DemoApi.Data.EF\Verisoft.DemoApi.Data.EF.csproj --startup-project C:\\...\Verisoft.DemoApi\Verisoft.DemoApi.Host\Verisoft.DemoApi.Host.csproj`

Spuštění migrace při startupu zajišťuje 
`Verisoft.Core.AspNet.ApplicationBuilderExtensions.MigrateDatabase<TDBContext>(this IApplicationBuilder app)`

``` csharp
public static void UseDemoApi(this IApplicationBuilder applicationBuilder)
{
    applicationBuilder.MigrateDatabase<IDemoApiDbContext>();
}
```