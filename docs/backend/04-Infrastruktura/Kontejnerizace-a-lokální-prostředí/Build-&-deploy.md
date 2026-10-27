# Build a deployment

## Lokální prostředí přes docker-compose

Kořenový [`docker-compose.yml`](../../../../docker-compose.yml) (a
`docker-compose.dcproj` pro spuštění z Visual Studia) definuje kompletní
lokální stack:

- `api` – `Verisoft.DemoApi.Host` (Dockerfile
  `src/Verisoft.DemoApi/Verisoft.DemoApi.Host/Dockerfile`, build context `./src`),
  prostředí `ASPNETCORE_ENVIRONMENT=DevelopmentDocker`.
- `codebookapi` – samostatný `Verisoft.CodebookApi.Host` (viz [Číselníky](../../02-API/Číselníky.md)).
- `db` – SQL Server (vlastní image z `deploy/db`), healthcheck přes `sqlcmd`.
- `rabbitmq` – RabbitMQ s management pluginem pro MassTransit (porty `5672`/`15672`).
- `azurite` – emulátor Azure Blob Storage pro [Blob storage](../Blob-storage-(práce-se-soubory).md).
- `keyvault` (+ `keyvault-cert-init`, `keyvault-seed`) – emulátor Azure Key
  Vaultu (`jamesgoulddev/azure-keyvault-emulator`); `keyvault-cert-init`
  vygeneruje self-signed certifikát, `keyvault-seed` po startu naplní
  potřebné tajné hodnoty (connection stringy, API secret, Application
  Insights connection string, AzureAd hodnoty) přes Key Vault REST API.
- `frontend` – `Verisoft.Frontend` (mimo rozsah této backendové dokumentace).
- `router` – nginx reverse proxy (`deploy/router`), publikuje port `80` a
  směruje na `api`, `codebookapi` a `frontend`.

Spuštění celého stacku:

```powershell
docker compose up --build
```

`api`/`codebookapi` čekají (`depends_on` + `condition`) na `db`/`rabbitmq`
(zdravé) a `keyvault-seed` (dokončené naplnění secrets), takže není nutné nic
startovat ručně v určitém pořadí.

## CI build

Demo API pipeline je
[`build/Verisoft.DemoApi.CI.yml`](../../../build/Verisoft.DemoApi.CI.yml).
Používá .NET SDK `10.x`, obnoví závislosti, sestaví
`src/Verisoft.DemoApi.sln` v konfiguraci `Release` a spustí testy.

Lokální ekvivalent:

```powershell
dotnet restore src/Verisoft.DemoApi.sln
dotnet build src/Verisoft.DemoApi.sln --configuration Release
dotnet test src/Verisoft.DemoApi.sln --configuration Release
```

## Docker image

Docker pipeline je
[`build/Verisoft.DemoApi.Docker.yml`](../../../build/Verisoft.DemoApi.Docker.yml).
Používá Dockerfile
`src/Verisoft.DemoApi/Verisoft.DemoApi.Host/Dockerfile` a build context `src`.
Tag image je číslo Azure DevOps buildu; registrace do container registry je
konfigurovaná v pipeline a nesmí obsahovat přihlašovací údaje v repozitáři.

## Deployment

Konkrétní Kubernetes manifesty nejsou součástí aktuálního backendového stromu.
Při nasazení musí deployment definovat image, namespace, port, prostředí
`ASPNETCORE_ENVIRONMENT`, resource requests/limits a health probe na `/health`.
Hodnoty prostředí a tajemství dodává deployment platforma nebo Key Vault (v
lokálním prostředí tuto roli hraje `keyvault`/`keyvault-seed` z
docker-compose výše).
