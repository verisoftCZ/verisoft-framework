#Vytvoření nového komplexního číselníku

Komplexní číselník je typ dědící od `CodebookRecordType`, který navíc
definuje vlastní veřejné property. Tyto property se v DB uloží jako owned
type `AdditionalProperties` entity `CodebookRecordEntity<T>` a v API se
serializují do `additionalProperties` v `StringValueDetail<T>`.

**1. Definice typu**

``` csharp
using Verisoft.Core.Common.Entities.Codebooks;

namespace Verisoft.DemoApi.Common.Entities.CodebookRecords;

public class Country : CodebookRecordType
{
    public string IsoAlpha2 { get; set; }

    public string IsoAlpha3 { get; set; }

    public string IsoNumeric3 { get; set; }

    public static CodebookRecord<Country> Czechia => new("Czechia");
}
```

**2. Registrace** – stejně jako u jednoduchého číselníku, buď automaticky přes
`CodebookDiscovery.DiscoverFromAssembly()`, nebo explicitně:

``` csharp
builder.Services.AddCodebooksForConsumer(
    CodebookDiscovery.DiscoverFromAssembly(typeof(CodebookParentEntity).Assembly));
```

Pro doplňující konfiguraci konkrétního číselníku (vlastní repository/service,
vlastní endpointy, oprávnění) lze při sestavování builderu použít
`MainCodebookRecordConfigurationBuilder.ForType<TCodebookRecordType>()`:

``` csharp
mainCodebookRecordConfigurationBuilder.ForType<Country>(config =>
{
    config.AddGlobalReadPermission("Codebooks.Country.Read");
    config.WithCustomEndpoint("Bulk", HttpMethod.Post, BulkAddHandler);
});
```

**3. Migrace** – po přidání typu je potřeba vytvořit a aplikovat EF Core
migraci pro příslušný `DbContext` (viz [Db konfigurace](../../03-Data/Databaze/Db-konfigurace.md)).

> Pozn.: Ruční zakládání entit v `CodebookApi.Core.Entities.ComplexCodebookRecordsEntities`,
> vlastní DTO třídy implementující `ITenantContract`/`TenantId`/`IsForbidden`
> a registrace přes `AddComplexCodebook<TEntity, TModel, TTenantModel>()`
> popsané dříve v tomto dokumentu neodpovídají aktuálnímu kódu – tyto typy a
> metody v `Verisoft.Core.Codebooks*` neexistují.
