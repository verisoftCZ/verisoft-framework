#Vytvoření nového jednoduchého číselníku

Jednoduchý číselník je typ dědící od `Verisoft.Core.Common.Entities.Codebooks.CodebookRecordType`,
který nedefinuje žádné vlastní veřejné property. Jednotlivé hodnoty číselníku
jsou statické instance `CodebookRecord<T>`.

**1. Definice typu** – nejlépe v `*.Common` projektu konzumujícího API
(`Entities/CodebookRecords/`):

``` csharp
using Verisoft.Core.Common.Entities.Codebooks;

namespace Verisoft.DemoApi.Common.Entities.CodebookRecords;

public class Gender : CodebookRecordType
{
    public static CodebookRecord<Gender> Male => new("Male");

    public static CodebookRecord<Gender> Female => new("Female");
}
```

**2. Registrace** – buď explicitně na `IServiceCollection` (viz
`RegisterCodebooks`), nebo automatickým vyhledáním všech `CodebookRecordType`
v assembly přes `CodebookDiscovery.DiscoverFromAssembly()` (tímto způsobem
registruje číselníky `Verisoft.DemoApi.Host`):

``` csharp
builder.Services.AddCodebooksForConsumer(
    CodebookDiscovery.DiscoverFromAssembly(typeof(CodebookParentEntity).Assembly));
```

Registrace přidá typ do `ICodebookRegistry`, zaregistruje repository, service
(`ICodebookRecordService<Gender>`, `IStringValueService<Gender>`) a
připraví EF konfiguraci; endpointy se vytvoří voláním
`WebApplicationExtensions.ConfigureEndpointsV1<Gender>()` v `Program.cs`
hostitelské aplikace (viz [Get endpointy](Get-endpointy.md)).

**3. Migrace** – po přidání nového typu je potřeba vytvořit a aplikovat EF Core
migraci pro příslušný `DbContext` (viz [Db konfigurace](../../03-Data/Databaze/Db-konfigurace.md)).

> Pozn.: Vlastnosti `IsBasicType`/`HasHardcodedEnum` a ruční zakládání entit
> přes `CodebookApi.Core.Entities.BasicCodebookRecordEntities` popsané dříve
> v tomto dokumentu neodpovídají aktuálnímu kódu – konfigurace číselníku se
> dnes odvozuje z typu `CodebookRecordType` a jeho registrace, nikoli z
> ručně zakládaných entit přes `CodebookController`.
