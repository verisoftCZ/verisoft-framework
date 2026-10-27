# Struktura backendu

Backend je rozdělený na znovupoužitelné knihovny `Verisoft.Core.*` a ukázkovou
aplikaci `Verisoft.DemoApi.*`. Demo API používá vrstvenou strukturu:

```text
src/Verisoft.DemoApi/
├── Verisoft.DemoApi.Common/       doménové entity, repository kontrakty a specifikace
├── Verisoft.DemoApi.Contracts/    API DTO, filtry, commandy a saga states
├── Verisoft.DemoApi.Data.EF/      DbContext, EF konfigurace, repository implementace a migrace
├── Verisoft.DemoApi.Application/  služby, mapování a validátory
├── Verisoft.DemoApi.Host/         startup, middleware, controllery, consumers a joby
├── Verisoft.DemoApi.Client/       generovaný klient
└── Verisoft.CodebookApi.Host/     samostatný host pro správu číselníků
```

## Core knihovny

Knihovny v `src/Verisoft.Core/` poskytují mimo jiné ASP.NET infrastrukturu,
autentizaci a oprávnění, EF Core/Dapper abstrakce, číselníky, MassTransit,
Hangfire, Blob Storage, exporty, PDF, šablony, OpenAPI a validaci.

Registrace služeb patří do existujících `InstallExtensions` tříd, nikoli přímo
do `Program.cs`, pokud pro daný modul už taková extension metoda existuje.

## Obecná pravidla

- Názvy tříd, metod a proměnných mají popisovat účel a respektují konvence C#.
- Business logika patří do Application služeb; controllery pouze zpracují HTTP
  vstup, zavolají službu a převedou výsledek na HTTP odpověď.
- Doménové entity a repository kontrakty patří do `Common`, persistence do
  `Data.EF` a API kontrakty do `Contracts`.
- Asynchronní operace používají `Async` API a umožňují předání
  `CancellationToken`, pokud to podporovaná vrstva vyžaduje.
- Novou změnu schématu vždy doplňte o EF Core migraci a migraci před commitnutím
  zkontrolujte.
