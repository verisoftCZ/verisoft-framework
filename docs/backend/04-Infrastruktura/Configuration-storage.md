# Úložiště konfigurací komponent

`Verisoft.Core.ConfigurationStorage` (.NET 8 a .NET 10) ukládá libovolné JSON
konfigurace FE komponent do jedné relační tabulky. Backend obsah neslučuje,
neinterpretuje ani nekonvertuje mezi verzemi schématu.

## Architektura

MVC controller volá `IConfigurationStorage`, jehož implementací je
`ConfigurationStorageService`. Služba řeší validaci, autentizaci, autorizaci
a sestavení odpovědi; nemá přímou závislost na DbContextu. Doménové operace jsou
upsert a čtení podle složeného klíče, nikoli obecné CRUD podle ID, proto služba
poskytuje vlastní rozhraní místo zpřístupnění generického CRUD API.

`IComponentConfigurationRepository` rozšiřuje frameworkové `IRepository`.
`ComponentConfigurationRepository` dědí z `EntityFrameworkRepositoryBase`
a řeší EF dotazy, parametrizaci dávek a persistenci. Upsert včetně commitu
a opakování při kolizi prvního zápisu je jedna repository operace. Mazání
používá `Remove` a služba následně volá `IUnitOfWork.CommitAsync`.

`ComponentConfiguration` dědí z `BaseEntity<Guid>`. Audit, soft delete,
query filter a rozšíření unikátních indexů zajišťuje
`EntityFrameworkDbContextBase`. Repository sdílí scoped kontext hostitele.
Repository je interní datová hranice aplikace, ne autorizované API; controllery
a další konzumenti musí přistupovat přes službu.

Chyby řeší globální handlery z `Verisoft.Core.AspNet`, registrované přes
`AddVerisoftExceptionHandlers`: `ValidationException` vrací 400,
`UnauthorizedAccessException` bezpečnou 403, neočekávané chyby nadále 500.
Mapování platí pro celou aplikaci, nikoli pouze konfigurace. Vlastní handlery
registrujte před frameworkovými; záchytný `GlobalExceptionHandler` musí být
poslední. Controller nemá vlastní exception filter; MVC a API versioning zůstávají.

## Datový model a pravidla

Tabulka `ComponentConfigurations` obsahuje `Id` (Guid), `Context`, `ComponentId`,
`ConfigurationType`, `Level`, `UserId`, `Data`, `SchemaVersion` a auditní pole
`CreatedBy`, `CreatedAt`, `UpdatedBy`, `UpdatedAt`, `IsDeleted` z `BaseEntity`.

| Pole | Pravidlo |
| --- | --- |
| Context | Aplikační kontext, 1 až 100 znaků; nastavuje hostitel, nikoli klient |
| ComponentId | Identifikátor konkrétní komponenty, 1 až 200 znaků |
| ConfigurationType | Nezávislý druh konfigurace, 1 až 100 znaků, např. table, export, filters |
| Level | Admin = 1 (ADMIN), User = 2 (USER) |
| UserId | Stabilní identifikátor přihlášeného uživatele, nejvýše 200 znaků; pro ADMIN vždy null |
| Data | Libovolná syntakticky platná JSON hodnota; v API je JSON, nikoli escapovaný řetězec |
| SchemaVersion | Kladné číslo, výchozí 1; verzi formátu a její interpretaci vlastní FE |
| UpdatedAt | API vrací UTC čas posledního uložení: auditní UpdatedAt, nebo CreatedAt před první změnou |

Filtrované unikátní indexy zajišťují nejvýše jeden aktivní ADMIN záznam pro
`Context + ComponentId + ConfigurationType` a jeden USER záznam pro
`Context + ComponentId + ConfigurationType + UserId`. Frameworková konvence
do obou indexů přidává podmínku `IsDeleted = 0`.
Databázová omezení kontrolují soulad úrovně s vlastníkem a kladnou verzi schématu.
Mapování a DemoApi migrace cílí na SQL Server; relační testy používají SQLite.
Sloupce `Context`, `ComponentId`, `ConfigurationType` a `UserId` používají binární
case-sensitive kolaci, aby SQL Server porovnával klíče stejně jako přesné ordinal
porovnání v .NET. Identifikátor uživatele musí být stabilní; API odmítá identifikátory
a konfigurační klíče s okrajovými mezerami, protože SQL Server by je při rovnosti
řetězců mohl považovat za shodné.

PUT je upsert: vytvoří záznam, nebo nahradí celý JSON a verzi existujícího záznamu,
přičemž zachová jeho `Id`. Při kolizi dvou prvních uložení unikátní index zabrání
duplicitě a repository zopakuje operaci jako aktualizaci. Souběžné změny nemají ETag
ani merge; vyhrává poslední úspěšný zápis. Historie změn JSONu se neukládá.
`SchemaVersion` neznamená historii záznamu, ale verzi formátu JSONu.

DELETE používá frameworkový soft delete: řádek zůstává v databázi s auditem
mazání, ale čtení ho nevrací. Další PUT vytvoří nový aktivní řádek s novým `Id`.
Smazaný JSON tedy zůstává uložený; případná retenční politika je odpovědností
hostitelské aplikace.

Auditní `CreatedBy` a `UpdatedBy` plní framework přes `IUserContext.GetUserName()`.
Nejsou autorizačním klíčem: vlastnictví USER konfigurace nadále určuje stabilní
`UserId` z claimu. Změna zobrazovaného jména ani jiný administrátor při změně
ADMIN konfigurace nesmí změnit její vlastnictví.

USER operace vždy používají vlastníka z autentizačního kontextu `IUserContext`.
API nepřijímá `UserId`, `Context` ani cizí ID záznamu jako cíl operace.
ADMIN konfigurace jsou čitelné všem přihlášeným uživatelům ve stejném kontextu;
jejich zápis a mazání vyžadují politiku `ConfigurationStorage.Admin`, výchozí roli
`Admin`. Toto pravidlo kontroluje controller i služba při přímém použití přes DI.

Kontext je v této verzi konfigurace hostitelské aplikace, ne automaticky tenant.
Pro multitenantní aplikaci je třeba zajistit důvěryhodné oddělení kontextu;
nepoužívejte neověřenou hodnotu z FE jako autorizační hranici.
Používejte stabilní, konzistentně psané klíče a identifikátory uživatelů.

## Souběh USER a ADMIN konfigurace

Pro stejnou komponentu a typ konfigurace mohou a mají existovat obě úrovně.
Například tyto záznamy jsou platné současně:

| Context | ComponentId | ConfigurationType | Level | UserId | Data |
| --- | --- | --- | --- | --- | --- |
| DemoApi | users-list | table | ADMIN | null | { "pageSize": 50 } |
| DemoApi | users-list | table | USER | user-456 | { "pageSize": 25 } |
| DemoApi | users-list | table | USER | user-789 | { "pageSize": 100 } |

Při čtení `users-list/table` dostane `user-456` ADMIN konfiguraci s hodnotou 50
a svou USER konfiguraci s hodnotou 25. FE může upřednostnit vlastní nastavení 25.
Uživatel `user-789` dostane stejnou ADMIN konfiguraci a svou USER hodnotu 100.
Uživatel bez vlastní konfigurace dostane ADMIN konfiguraci a `user: null`.
Po smazání vlastní konfigurace může FE znovu použít ADMIN nastavení.
Backend tyto hodnoty neslučuje ani za FE nevybírá výsledek.

Pořadí vytvoření úrovní nehraje roli: ADMIN lze uložit před USER i po něm.
Zápis jedné úrovně nemění druhou. Opakovaný zápis stejného klíče ve stejné
úrovni a pro stejného vlastníka aktualizuje existující záznam místo vytvoření dalšího.

To se liší od správy více pojmenovaných presetů, např. "Project Configuration".
Naše konfigurace nemá pole `Name`; `ComponentId` ani `ConfigurationType` nejsou
názvy presetů, ale stabilní technické klíče. Nezakazujeme tedy shodu těchto klíčů
mezi USER a ADMIN a nepřidáváme přípony jako " - System Configuration".
Takový zákaz by znemožnil uživatelské nastavení nad společným ADMIN základem.
Pokud by JSON obsahoval vlastní pole s názvem, backend je neinterpretuje
a jeho unikátnost nekontroluje. Více pojmenovaných presetů a pravidla jejich
názvů by byly samostatným rozšířením datového modelu a API.

## K čemu slouží Context

`Context` je jmenný prostor hostitelské aplikace v úložišti. Umožňuje oddělit
konfigurace více aplikací, které sdílejí stejnou databázovou tabulku a používají
stejné technické klíče. Například oba tyto ADMIN záznamy mohou existovat současně:

| Context | ComponentId | ConfigurationType | Level | UserId | Data |
| --- | --- | --- | --- | --- | --- |
| SalesApp | users-list | table | ADMIN | null | { "pageSize": 25 } |
| SupportApp | users-list | table | ADMIN | null | { "pageSize": 100 } |

SalesApp zaregistruje úložiště s `options.Context = "SalesApp"`, SupportApp
s `options.Context = "SupportApp"`. Každá pak přes úložiště čte a mění pouze
své záznamy, i když obě požadují `users-list/table`. Stejné oddělení platí pro
USER konfigurace, i když by obě aplikace používaly shodný `UserId`.

Hodnota se nastavuje na serveru při registraci služby, nikoli v HTTP požadavku.
DemoApi používá `"DemoApi"`, výchozí hodnota knihovny je `"default"`.
`Context` je součástí unikátních indexů i podmínek pro čtení, zápis a mazání.
Není to EF `DbContext`, databázové schéma ani automaticky identifikátor tenanta.
Jde o logické oddělení dat v této službě, nikoli náhradu autentizace, autorizace
nebo databázových oprávnění. Samotný `Context` nezajišťuje multitenantní bezpečnost.

Pro jednu aplikaci s vlastní databází není další jmenný prostor nezbytný;
v této implementaci zůstává součástí klíče kvůli možnosti sdílet tabulku mezi
aplikacemi. Pokud více aplikací použije stejný `Context` ve stejné tabulce,
budou záměrně pracovat se stejným prostorem konfigurací.

## Zapojení do jiné aplikace

Přidejte referenci na knihovnu a EF SQL Server provider odpovídající verze.
Hostitelský kontext musí dědit z `EntityFrameworkDbContextBase` s řádně
inicializovanými `IUserContext`, `IWalkerFactory` a `TimeProvider`, jako DemoApi.
Samostatný `DbContext` již není podporován, protože neposkytuje frameworkový
audit a soft delete.
Zaregistrujte existující `IUserContext`, který vrací přihlášený `ClaimsPrincipal`,
controller services a úložiště:

```csharp
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Verisoft.Core.AspNet;
using Verisoft.Core.ConfigurationStorage;
using Verisoft.Core.Data.EntityFramework.Walkers;

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddVerisoftExceptionHandlers();
builder.Services.AddScoped<IWalkerFactory, WalkerFactory>();
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

Výchozí claim je `ClaimTypes.NameIdentifier`. Pokud autentizace ponechává
nemapované JWT claims, nastavte např. `options.UserIdClaimType = "sub"`.
Claim musí být stabilní a jednoznačný v rámci aplikačního kontextu; samotné jméno
nebo e-mail se jako náhrada identifikátoru nepoužívají. Pokud přijímáte tokeny více
issuerů, musí jejich identifikátory být v tomto kontextu jednoznačné.
Politiku lze změnit např. na `policy.RequireClaim("permission", "configuration.admin")`.

DbContext explicitně připojí model před základním `OnModelCreating`, aby
framework našel entitu při nastavení soft-delete query filtru:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.AddConfigurationStorage();
    base.OnModelCreating(modelBuilder);
}
```

Zpřístupněte zděděný controller na vlastní route. V aplikaci s API versioningem
přidejte také odpovídající `[ApiVersion]`, jako v DemoApi:

```csharp
using Microsoft.AspNetCore.Mvc;
using Verisoft.Core.ConfigurationStorage;

[Route("component-configurations")]
public sealed class ComponentConfigurationsController(IConfigurationStorage storage)
    : ConfigurationStorageControllerBase(storage);
```

Pipeline musí obsahovat `UseExceptionHandler()` před autentizací, autorizací
a `MapControllers()`.
Při registraci DbContextu přes vlastní rozhraní použijte parametr `contextFactory`,
aby úložiště sdílelo stejnou scoped instanci. DemoApi tak používá `IDemoApiDbContext`.
Při přímém volání `IConfigurationStorage` obslužte `ValidationException` a
`UnauthorizedAccessException`; HTTP middleware je přes globální handlery mapuje na 400 a 403.
Používejte krátké scoped instance DbContextu, typicky jednu pro každý požadavek.

## DemoApi a migrace

DemoApi používá kontext `DemoApi` a prefix `/api/v2/component-configurations`.
Lokální launch profil poslouchá na `http://localhost:6389`.
Databáze a ostatní závislosti DemoApi musí být dostupné jako při běžném spouštění.

Migrace `20261103143146_AddComponentConfigurationStorage` vytváří tabulku, indexy
a kontrolní omezení. Navazující `20261105161804_ConfigurationStorageBinaryCollation`
nastavuje binární kolaci klíčových sloupců a obnovuje unikátní indexy.
Jejich `Down` operace odstraní tabulku, respektive vrátí původní kolaci.
Migrace `20261105220323_ConfigurationStorageFrameworkEntity` přidává audit
a soft delete a upravuje indexy přes frameworkovou konvenci. U starých řádků
přebírá poslední známý čas do `CreatedAt` a nastaví `CreatedBy` na
`configuration-storage:migration`; skutečný čas vytvoření ani původní autor
nejsou zpětně známí. `UserId` a JSON zůstávají beze změny.
Její `Down` trvale odstraní soft-deleted řádky (aby se neobnovily smazané
konfigurace a nekolidovaly původní indexy), zachová aktivní řádky a jejich
poslední čas uložení a odebere auditní sloupce. Před rollbackem zálohujte data.
Příkazy spouštějte z kořene repozitáře:

```powershell
dotnet ef database update --project src/Verisoft.DemoApi/Verisoft.DemoApi.Data.EF --startup-project src/Verisoft.DemoApi/Verisoft.DemoApi.Data.EF --context DemoApiDbContext
dotnet run --project src/Verisoft.DemoApi/Verisoft.DemoApi.Host
```

Design-time factory načte `Database:ConnectionString` z user secrets datového
projektu nebo z proměnné prostředí `Database__ConnectionString`. Volitelně lze
předat connection string jako poslední argument za `--`. Při update se aplikují
i dosud neprovedené starší migrace. Nová aplikace si vygeneruje vlastní migraci
ze svého DbContextu; DemoApi migrace nejsou součástí knihovny.

Autentizace používá `Authorization: Bearer <token>`. Existující demo endpoint
`GET /api/Auth/token?email=<email existujiciho uzivatele>` vrací token v poli `token`.
Pozor: současný demo `TokenService` přidává roli `Admin` každému tokenu. Takový
token tedy neověří zamítnutí ADMIN zápisu běžnému uživateli. Pro tento scénář
použijte důvěryhodně vydaný token bez této role; automatické HTTP testy už ověřují
obě identity samostatně. Knihovna sama roli žádnému uživateli nepřiděluje.

## HTTP API

Relativní cesty v tabulce navazují na `/api/v2/component-configurations`:

| Metoda | Cesta | Výsledek |
| --- | --- | --- |
| GET | /{componentId}/{configurationType} | 200, odděleně ADMIN a vlastní USER |
| POST | /query | 200, dávkové čtení dvojic komponenta/typ bez limitu počtu položek |
| PUT | /{componentId}/{configurationType}/user | 200, upsert vlastní konfigurace |
| PUT | /{componentId}/{configurationType}/admin | 200, upsert sdílené konfigurace; pouze administrátor |
| DELETE | /{componentId}/{configurationType}/user | 204, nebo 404 pokud vlastní záznam neexistuje |
| DELETE | /{componentId}/{configurationType}/admin | 204, nebo 404; pouze administrátor |

Dávkový endpoint nemá limit počtu klíčů. Backend dotaz rozděluje do bloků po 500
požadovaných dvojicích, takže nevytváří kartézské kombinace komponent a typů
a drží počet SQL parametrů na blok v limitu SQL Serveru. Odpověď zachovává pořadí
vstupu; opakovaný klíč se ve výsledku objeví opakovaně.

Nepřihlášený klient dostane 401, nedostatečné oprávnění nebo chybějící stabilní
identifikátor 403. Chybný JSON, chybějící `data`, nekladná verze, neplatný klíč
nebo neplatná dávka vracejí 400. JSON hodnota `null` je platný obsah konfigurace
a není totéž co chybějící konfigurace.

Uložení vlastní konfigurace:

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

Pro ADMIN použijte stejné tělo a cestu končící `/admin`.
Pro novou verzi formátu pošlete nový JSON a vyšší `schemaVersion` stejným PUT.
Různé typy, např. `table`, `export` a `filters`, se ukládají nezávisle.

Dávkové čtení:

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

Odpověď obsahuje jednu položku pro každou požadovanou dvojici ve stejném pořadí.
Prázdné pole `configurations` je platné a vrací prázdné pole výsledků.
GET vrací stejnou strukturu, ale jako jeden objekt místo pole.
DemoApi serializuje `level` jako řetězec `Admin` nebo `User`; úrovně jsou současně
jednoznačně rozlišené poli `admin` a `user`.

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

Pokud konfigurace neexistují, HTTP čtení stále vrací 200 s `admin: null` a
`user: null`. Nikdy nevrací konfiguraci jiného uživatele.

## Příklad FE

Jednoduchá volba celé konfigurace respektující prioritu USER před ADMIN:

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

Tento příklad nahrazuje celou konfiguraci. Skládání jednotlivých vlastností,
podpora konkrétních verzí schématu a případná migrace JSONu jsou odpovědností FE.
Backend neposkytuje sloučený výsledek. Po smazání USER konfigurace tak FE může
znovu použít ADMIN; bez obou úrovní použije nastavení definované v kódu.
`filters` znamená dlouhodobé přednastavení, nikoli aktuální hodnoty filtrů;
ty zůstávají v URL.

Kompletní požadavky jsou v souboru
[ComponentConfigurations.http](../../../src/Verisoft.DemoApi/Verisoft.DemoApi.Host/ComponentConfigurations.http).

## Testy

```powershell
dotnet test src/Verisoft.Core/Verisoft.Core.ConfigurationStorage.Test/Verisoft.Core.ConfigurationStorage.Test.csproj
```

NUnit testy běží na .NET 8 a .NET 10 bez externí databáze. Pokrývají persistenci,
upsert včetně kolize prvních zápisů, omezení vlastníků a unikátnosti, izolaci
uživatelů i aplikačních kontextů, autorizaci ADMIN operací, libovolný JSON,
verzi schématu, frameworkový audit, oddělení autora od vlastníka, soft delete
a opětovné vytvoření klíče, chybějící konfigurace a dávkové čtení.
HTTP testy navíc ověřují routování, model binding, stavové kódy 400/401/403/404,
ignorování podstrčeného `UserId` a celý cyklus uložení/načtení/smazání.
Globální handlery se testují také na controlleru mimo úložiště, včetně
bezpečné odpovědi 500 bez zveřejnění textu neočekávané výjimky.