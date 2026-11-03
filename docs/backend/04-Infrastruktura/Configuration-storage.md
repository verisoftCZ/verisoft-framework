# Uloziste konfiguraci komponent

`Verisoft.Core.ConfigurationStorage` (.NET 8 a .NET 10) uklada libovolne JSON
konfigurace FE komponent do jedne relacni tabulky. Backend obsah neslucuje,
neinterpretuje ani nekonvertuje mezi verzemi schematu.

## Datovy model a pravidla

Tabulka `ComponentConfigurations` obsahuje `Id` (Guid), `Context`, `ComponentId`,
`ConfigurationType`, `Level`, `UserId`, `Data`, `SchemaVersion` a `UpdatedAt` (UTC).

| Pole | Pravidlo |
| --- | --- |
| Context | Aplikacni kontext, 1 az 100 znaku; nastavuje hostitel, nikoli klient |
| ComponentId | Identifikator konkretni komponenty, 1 az 200 znaku |
| ConfigurationType | Nezavisly druh konfigurace, 1 az 100 znaku, napr. table, export, filters |
| Level | Admin = 1 (ADMIN), User = 2 (USER) |
| UserId | Stabilni identifikator prihlaseneho uzivatele, nejvyse 200 znaku; pro ADMIN vzdy null |
| Data | Libovolna syntakticky platna JSON hodnota; v API je JSON, nikoli escapovany retezec |
| SchemaVersion | Kladne cislo, vychozi 1; verzi formatu a jeji interpretaci vlastni FE |
| UpdatedAt | Cas posledniho ulozeni; urcuje server |

Filtrovane unikatni indexy zajistuji nejvyse jeden ADMIN zaznam pro
`Context + ComponentId + ConfigurationType` a jeden USER zaznam pro
`Context + ComponentId + ConfigurationType + UserId`.
Databazova omezeni kontroluji soulad urovne s vlastnikem a kladnou verzi schematu.
Mapovani a DemoApi migrace cili na SQL Server; relacni testy pouzivaji SQLite.

PUT je upsert: vytvori zaznam, nebo nahradi cely JSON a verzi existujiciho zaznamu,
pricemz zachova jeho `Id`. Pri kolizi dvou prvnich ulozeni unikatni index zabrani
duplicite a sluzba zopakuje operaci jako aktualizaci. Soubazne zmeny nemaji ETag
ani merge; vyhrava posledni uspesny zapis. Historie konfiguraci se neuklada.
`SchemaVersion` neznamena historii zaznamu, ale verzi formatu JSONu.

USER operace vzdy pouzivaji vlastnika z autentizacniho kontextu `IUserContext`.
API neprijima `UserId`, `Context` ani cizi ID zaznamu jako cil operace.
ADMIN konfigurace jsou citelne vsem prihlasenym uzivatelum ve stejnem kontextu;
jejich zapis a mazani vyzaduji politiku `ConfigurationStorage.Admin`, vychozi roli
`Admin`. Toto pravidlo kontroluje controller i sluzba pri primem pouziti pres DI.

Kontext je v teto verzi konfigurace hostitelske aplikace, ne automaticky tenant.
Pro multitenantni aplikaci je treba zajistit duveryhodne oddeleni kontextu;
nepouzivejte neoverenou hodnotu z FE jako autorizacni hranici.
Pouzivejte stabilni, konzistentne psane klice a identifikatory uzivatelu.

## Zapojeni do jine aplikace

Pridejte referenci na knihovnu a EF SQL Server provider odpovidajici verze.
Zaregistrujte existujici `IUserContext`, ktery vraci prihlaseny `ClaimsPrincipal`,
controller services a uloziste:

```csharp
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Verisoft.Core.ConfigurationStorage;

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));
builder.Services.AddConfigurationStorage<AppDbContext>(
    options =>
    {
        options.Context = "MyApplication";
        options.UserIdClaimType = ClaimTypes.NameIdentifier;
    },
    configureAdminPolicy: policy => policy.RequireRole("Admin"));
```

Vychozi claim je `ClaimTypes.NameIdentifier`. Pokud autentizace ponechava
nemapovane JWT claims, nastavte napr. `options.UserIdClaimType = "sub"`.
Claim musi byt stabilni a jednoznacny v ramci aplikacniho kontextu; samotne jmeno
nebo e-mail se jako nahrada identifikatoru nepouzivaji. Pokud prijimate tokeny vice
issueru, musi jejich identifikatory byt v tomto kontextu jednoznacne.
Politiku lze zmenit napr. na `policy.RequireClaim("permission", "configuration.admin")`.

DbContext explicitne pripoji model:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
    modelBuilder.AddConfigurationStorage();
}
```

Zpristupnete zdedeny controller na vlastni route. V aplikaci s API versioningem
pridejte take odpovidajici `[ApiVersion]`, jako v DemoApi:

```csharp
using Microsoft.AspNetCore.Mvc;
using Verisoft.Core.ConfigurationStorage;

[Route("component-configurations")]
public sealed class ComponentConfigurationsController(IConfigurationStorage storage)
    : ConfigurationStorageControllerBase(storage);
```

Pipeline musi obsahovat autentizaci, autorizaci a `MapControllers()`.
Pri registraci DbContextu pres vlastni rozhrani pouzijte parametr `contextFactory`,
aby uloziste sdilelo stejnou scoped instanci. DemoApi tak pouziva `IDemoApiDbContext`.
Pri primem volani `IConfigurationStorage` obsluzte `ValidationException` a
`UnauthorizedAccessException`; HTTP controller je mapuje na 400 a 403.
Pouzivejte kratke scoped instance DbContextu, typicky jednu pro kazdy pozadavek.

## DemoApi a migrace

DemoApi pouziva kontext `DemoApi` a prefix `/api/v2/component-configurations`.
Lokalni launch profil posloucha na `http://localhost:6389`.
Databaze a ostatni zavislosti DemoApi musi byt dostupne jako pri beznem spousteni.

Migrace `20261103143146_AddComponentConfigurationStorage` vytvari pouze novou
tabulku, indexy a kontrolni omezeni. `Down` tuto tabulku odstrani vcetne jejiho obsahu.
Prikazy spoustejte z korene repozitare:

```powershell
dotnet ef database update --project src/Verisoft.DemoApi/Verisoft.DemoApi.Data.EF --startup-project src/Verisoft.DemoApi/Verisoft.DemoApi.Data.EF --context DemoApiDbContext
dotnet run --project src/Verisoft.DemoApi/Verisoft.DemoApi.Host
```

Design-time factory nacte `Database:ConnectionString` z user secrets datoveho
projektu nebo z promenne prostredi `Database__ConnectionString`. Volitelne lze
predat connection string jako posledni argument za `--`. Pri update se aplikuji
i dosud neprovedene starsi migrace. Nova aplikace si vygeneruje vlastni migraci
ze sveho DbContextu; DemoApi migrace nejsou soucasti knihovny.

Autentizace pouziva `Authorization: Bearer <token>`. Existujici demo endpoint
`GET /api/Auth/token?email=<email existujiciho uzivatele>` vraci token v poli `token`.
Pozor: soucasny demo `TokenService` pridava roli `Admin` kazdemu tokenu. Takovy
token tedy neoveri zamitnuti ADMIN zapisu beznemu uzivateli. Pro tento scenar
pouzijte duveryhodne vydany token bez teto role; automaticke HTTP testy uz overuji
obe identity samostatne. Knihovna sama roli zadnemu uzivateli neprideluje.

## HTTP API

Relativni cesty v tabulce navazuji na `/api/v2/component-configurations`:

| Metoda | Cesta | Vysledek |
| --- | --- | --- |
| GET | /{componentId}/{configurationType} | 200, oddelene ADMIN a vlastni USER |
| POST | /query | 200, davkove cteni dvojic komponenta/typ bez limitu poctu polozek |
| PUT | /{componentId}/{configurationType}/user | 200, upsert vlastni konfigurace |
| PUT | /{componentId}/{configurationType}/admin | 200, upsert sdilene konfigurace; pouze administrator |
| DELETE | /{componentId}/{configurationType}/user | 204, nebo 404 pokud vlastni zaznam neexistuje |
| DELETE | /{componentId}/{configurationType}/admin | 204, nebo 404; pouze administrator |

Neprihlaseny klient dostane 401, nedostatecne opravneni nebo chybejici stabilni
identifikator 403. Chybny JSON, chybejici `data`, nekladna verze, neplatny klic
nebo neplatna davka vraceji 400. JSON hodnota `null` je platny obsah konfigurace
a neni totez co chybejici konfigurace.

Ulozeni vlastni konfigurace:

```http
PUT /api/v2/component-configurations/users-list/table/user
Authorization: Bearer <token>
Content-Type: application/json

{
  "data": {
    "columns": [
      { "key": "firstName", "visible": true, "order": 1, "width": 200 },
      { "key": "email", "visible": false, "order": 2 }
    ],
    "pageSize": 25,
    "density": "compact"
  },
  "schemaVersion": 1
}
```

Pro ADMIN pouzijte stejne telo a cestu koncici `/admin`.
Pro novou verzi formatu poslete novy JSON a vyssi `schemaVersion` stejnym PUT.
Ruzne typy, napr. `table`, `export` a `filters`, se ukladaji nezavisle.

Davkove cteni:

```http
POST /api/v2/component-configurations/query
Authorization: Bearer <token>
Content-Type: application/json

{
  "configurations": [
    { "componentId": "users-list", "configurationType": "table" },
    { "componentId": "users-list", "configurationType": "export" },
    { "componentId": "dashboard-main", "configurationType": "widgets" }
  ]
}
```

Odpoved obsahuje jednu polozku pro kazdou pozadovanou dvojici ve stejnem poradi.
Prazdne pole `configurations` je platne a vraci prazdne pole vysledku.
GET vraci stejnou strukturu, ale jako jeden objekt misto pole.
DemoApi serializuje `level` jako retezec `Admin` nebo `User`; urovne jsou soucasne
jednoznacne rozlisene poli `admin` a `user`.

```json
{
  "componentId": "users-list",
  "configurationType": "table",
  "admin": null,
  "user": {
    "id": "52771fbc-9fe0-4b9d-bcb1-38dfce181624",
    "level": "User",
    "data": { "pageSize": 25 },
    "schemaVersion": 1,
    "updatedAt": "2026-11-03T12:00:00+00:00"
  }
}
```

Pokud konfigurace neexistuji, HTTP cteni stale vraci 200 s `admin: null` a
`user: null`. Nikdy nevraci konfiguraci jineho uzivatele.

## Priklad FE

Jednoducha volba cele konfigurace respektujici prioritu USER pred ADMIN:

```javascript
const response = await fetch(
  "/api/v2/component-configurations/users-list/table",
  { headers: { Authorization: `Bearer ${token}` } }
);
if (!response.ok) throw new Error(`Configuration request failed: ${response.status}`);
const available = await response.json();
const selected = available.user ?? available.admin;
const configuration = selected ? selected.data : defaultsFromCode;
```

Tento priklad nahrazuje celou konfiguraci. Skladani jednotlivych vlastnosti,
podpora konkretnich verzi schematu a pripadna migrace JSONu jsou odpovednosti FE.
Backend neposkytuje slouceny vysledek. Po smazani USER konfigurace tak FE muze
znovu pouzit ADMIN; bez obou urovni pouzije nastaveni definovane v kodu.
`filters` znamena dlouhodobe prednastaveni, nikoli aktualni hodnoty filtru;
ty zustavaji v URL.

Kompletni pozadavky jsou v souboru
[ComponentConfigurations.http](../../../src/Verisoft.DemoApi/Verisoft.DemoApi.Host/ComponentConfigurations.http).

## Testy

```powershell
dotnet test src/Verisoft.Core/Verisoft.Core.ConfigurationStorage.Test/Verisoft.Core.ConfigurationStorage.Test.csproj
```

NUnit testy bezi na .NET 8 a .NET 10 bez externi databaze. Pokryvaji persistence,
upsert vcetne kolize prvnich zapisu, omezeni vlastniku a unikatnosti, izolaci
uzivatelu i aplikacnich kontextu, autorizaci ADMIN operaci, libovolny JSON,
verzi schematu, mazani, chybejici konfigurace a davkove cteni.
HTTP testy navic overuji routovani, model binding, stavove kody 400/401/403/404,
ignorovani podstrceneho `UserId` a cely cyklus ulozeni/nacteni/smazani.