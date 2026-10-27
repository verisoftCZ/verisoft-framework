# Čistota kódu

## Statická analýza

Kvalita kódu je vynucována přes StyleCop analyzer a společný ruleset
[`src/.ruleset`](../../../src/.ruleset), na který se odkazuje `CodeAnalysisRuleSet`
prakticky ve všech `.csproj` souborech `Verisoft.Core.*` i `Verisoft.DemoApi.*`.
Vybraná pravidla jsou vypnutá (např. `SA1101` – prefix `this.`, `SA1600`/`SA1601`/
`SA1602`/`SA1633` – povinná XML dokumentace), naopak `CS8019` (nepoužité
`using`) je zapnuté jako warning a `CA1802` jako error.

Všechny projekty mají zapnuté:

```xml
<TargetFramework>net10.0</TargetFramework>
<Nullable>enable</Nullable>
<ImplicitUsings>enable</ImplicitUsings>
```

Nullable reference types jsou tedy vynucené v celém frameworku – nový kód
nemá potlačovat varování `!`/`#nullable disable` bez opodstatnění.

## Obecné principy struktury kódu

Framework používá vertical slice architekturu s jasným rozdělením
odpovědností do projektů (viz [Backend](Backend.md)):

- **Common** – entity, repository interfaces, specifikace, sorty, enumy.
  Neobsahuje závislost na EF Core ani ASP.NET Core.
- **Contracts** – DTO modely (`{Entity}`, `{Entity}EditModel`,
  `{Entity}ListItem`), filtry, command zprávy, saga stavy. Bez business
  logiky.
- **Data.EF** – `DbContext`, EF konfigurace, repository implementace,
  migrace.
- **Application** – business logika, service třídy s interface (`Services/`
  + `Services/Interfaces/`), validátory (FluentValidation), base controllery.
- **Host** – ASP.NET Core host, controllery, MassTransit konzumenti, Hangfire
  joby, `InstallExtensions`.

Doporučené principy, které se v repozitáři dodržují a mají se dodržovat i
v novém kódu:

- **Registrace přes `InstallExtensions`** – veškerá registrace do DI
  kontejneru patří do statických `InstallExtensions` tříd (`AddXyz(this
  IServiceCollection ...)`), ne inline do `Program.cs`. Použití `TryAddScoped`/
  `TryAddSingleton` namísto `AddScoped`/`AddSingleton`, aby šlo výchozí
  implementaci v testech/konzumentech přepsat.
- **Repository pattern** – přístup k datům výhradně přes repozitáře
  (`IRepository<TEntity, TKey>`), nikdy přímo přes `DbContext` z aplikační
  vrstvy, viz [Repository pattern](../03-Data/Databaze/Repository-pattern.md).
- **Specifikace pro filtrování** – filtry z API se mapují (TypeMapperem) na
  `ISpecification<TEntity>` a teprve ta se převádí na LINQ expression
  (`SatisfiedBy()`), viz [Užití](../03-Data/Databaze/Užití.md).
- **Tenké controllery** – controllery pouze validují/mapují vstup, volají
  service a překládají `BusinessActionResult`/`BusinessResult` na
  `IActionResult` (`ToActionResult()`), business logika samotná patří do
  `Application/Services`, viz [Controllery](../02-API/Controllery.md).
- **`BusinessResult`/`BusinessActionResult`** – service metody vrací
  `BusinessResult`/`BusinessActionResult<T>` (`Verisoft.Core.Contracts.
  BusinessResult`) místo přímého vyhazování výjimek pro očekávané chybové
  stavy (validace, not found, forbidden apod.).
- **Žádná duplicitní infrastruktura** – auditní pole, soft delete a
  interceptory řeší `EntityFrameworkDbContextBase` automaticky (viz
  [Databáze](../03-Data/Databaze.md)); nový kód nemá psát vlastní implementaci téže
  věci, pokud ji framework už poskytuje.
- **Nullable reference types** – nové veřejné API má mít správně
  anotované nullable/non-nullable typy, ne jen `!`-potlačení varování.
- **Testovatelnost** – business logika v `Application` vrstvě má být
  testovatelná bez běžícího ASP.NET Core hostu ani reálné databáze (mock
  repozitářů/služeb), viz [Unit testy](unit-testy.md).