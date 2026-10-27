# Lokální prostředí

Backend lze spustit samostatně pomocí `dotnet run`, nebo v integrovaném Docker
Compose prostředí. Aktuální definice je vždy v kořenovém
[`docker-compose.yml`](../../../docker-compose.yml); tento dokument ji neduplikuje,
aby se příklady nerozcházely s konfigurací.

## Docker Compose

Z kořene repozitáře spusťte:

```powershell
docker compose up --build
```

Backendové služby v compose používají prostředí `DevelopmentDocker`, SQL Server,
RabbitMQ, lokální Azure Key Vault emulator a Azurite. API je dostupné přes
router na `/api`; health check je na `/health`.

## Samostatné spuštění API

```powershell
dotnet run --project src/Verisoft.DemoApi/Verisoft.DemoApi.Host
```

Při samostatném spuštění musí být dostupné závislosti a konfigurace, zejména
`Database:ConnectionString`, autentizace, RabbitMQ a případně Key Vault.

## Dockerfile

Dockerfile backendu je v
`src/Verisoft.DemoApi/Verisoft.DemoApi.Host/Dockerfile`. Build context je
`src`, proto se cesty v Dockerfile vztahují k této složce.
