# Verisoft Framework documentation

Tato složka obsahuje dokumentaci k backendové části Verisoft Frameworku a ukázkové
aplikaci Demo API. Frontendová dokumentace z původní wiki se do repozitáře
nepřevádí.

## Backend

- [Struktura backendu](backend/01-Architektura/Backend.md)
- [Aplikační vrstva a base služby](backend/01-Architektura/Aplikacni-vrstva-a-base-services.md)
- [Mapování a konfigurace AutoMapperu](backend/01-Architektura/Mapovani-a-konfigurace-AutoMapperu.md)
- [Čistota kódu](backend/01-Architektura/Cistota-kodu.md)
- [Unit testy](backend/01-Architektura/unit-testy.md)
- [Controllery](backend/02-API/Controllery.md)
- [Validace](backend/02-API/Validace.md)
- [Autentizace a autorizace](backend/02-API/Autentizace-a-autorizace.md)
- [Číselníky](backend/02-API/Číselníky.md)
- [Databáze a EF Core](backend/03-Data/Databaze.md)
- [Auditování](backend/03-Data/Auditovani.md)
- [Concurrency a zamykání záznamů](backend/03-Data/Concurrency.md)
- [Konfigurace](backend/04-Infrastruktura/Konfigurace.md)
- [Lokální prostředí a kontejnery](backend/04-Infrastruktura/Kontejnerizace-a-lokální-prostředí.md)
- [MassTransit a RabbitMQ](backend/04-Infrastruktura/MassTransit-RabbitMQ.md)
- [Monitoring a health checks](backend/04-Infrastruktura/Monitoring.md)
- [CRON a Hangfire](backend/04-Infrastruktura/CRON.md)
- [Blob storage](backend/04-Infrastruktura/Blob-storage-\(práce-se-soubory\).md)
- [NuGet balíčky](backend/04-Infrastruktura/Nugety.md)
- [Ověřené NuGet balíčky](backend/04-Infrastruktura/overene-nugety.md)
- [Notifikace](backend/04-Infrastruktura/notifikace.md)
- [Reporty](backend/05-Dokumenty/reporty.md)
- [CI/CD a deployment](backend/04-Infrastruktura/Kontejnerizace-a-lokální-prostředí/Build-&-deploy.md)

## Lokální backend

Z kořene repozitáře lze spustit integrované prostředí:

```powershell
docker compose up --build
```

Pro samotné Demo API:

```powershell
dotnet build src/Verisoft.DemoApi.sln
dotnet run --project src/Verisoft.DemoApi/Verisoft.DemoApi.Host
```

Příkazy a nastavení v dokumentaci jsou vztažené k aktuálnímu stromu `src/`.
Tajemství a produkční přihlašovací údaje do dokumentace nepatří.
