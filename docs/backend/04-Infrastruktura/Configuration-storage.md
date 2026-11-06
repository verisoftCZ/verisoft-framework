# Úložiště konfigurací komponent

`Verisoft.Core.ConfigurationStorage` ukládá JSON nastavení komponent, například
sloupce tabulky nebo velikost stránky. Pro každou dvojici `componentId` a
`configurationType` vrací společné nastavení **ADMIN** a vlastní nastavení **USER**.
Backend JSON neslučuje ani za frontend nevybírá, které nastavení použít.

Příklady používají DemoApi na `http://localhost:6389` a prefix
`/api/v2/component-configurations`. Všechna volání vyžadují Bearer token.
Spustitelné požadavky jsou v
[ComponentConfigurations.http](../../../src/Verisoft.DemoApi/Verisoft.DemoApi.Host/ComponentConfigurations.http).

## Uložení vlastního nastavení

Uživatel si nastaví tabulku `users-list`:

```http
PUT /api/v2/component-configurations/users-list/table/user
Authorization: Bearer <token>
Content-Type: application/json

{
  "data": { "pageSize": 25, "density": "compact" },
  "schemaVersion": 1
}
```

Odpověď `200 OK`:

```json
{
  "id": "52771fbc-9fe0-4b9d-bcb1-38dfce181624",
  "level": "User",
  "data": { "pageSize": 25, "density": "compact" },
  "schemaVersion": 1,
  "updatedAt": "2026-11-06T12:00:00+00:00"
}
```

PUT vytvoří konfiguraci, nebo **nahradí celý její JSON**. Při aktualizaci zachová
`id`; `updatedAt` označuje čas posledního uložení. Vlastníka určuje přihlášený
uživatel, proto se `userId` neposílá a nelze tak změnit cizí nastavení.

`schemaVersion` je verze formátu JSONu pro FE, nikoli číslo revize záznamu.
Musí být kladná, při vynechání má hodnotu `1`. Backend formát nepřevádí ani
nevyžaduje zvyšování verze při každém uložení.

## Uložení společného nastavení

Administrátor uloží výchozí nastavení stejné tabulky pro všechny uživatele:

```http
PUT /api/v2/component-configurations/users-list/table/admin
Authorization: Bearer <admin-token>
Content-Type: application/json

{
  "data": { "pageSize": 50, "density": "normal" },
  "schemaVersion": 1
}
```

Odpověď je opět `200 OK` se stejnou strukturou jako u USER, ale s vlastním `id`,
`level: "Admin"` a právě uloženými daty. USER nastavení se tím nezmění.
Zápis i mazání ADMIN vyžadují politiku `ConfigurationStorage.Admin`, ve výchozím
nastavení roli `Admin`; běžný uživatel dostane `403 Forbidden`.

## Načtení a použití nastavení

```http
GET /api/v2/component-configurations/users-list/table
Authorization: Bearer <token>
```

Po předchozích uloženích dostane jejich autor `200 OK`:

```json
{
  "componentId": "users-list",
  "configurationType": "table",
  "admin": {
    "id": "481aa833-e00c-40e7-8610-cec640ae1744",
    "level": "Admin",
    "data": { "pageSize": 50, "density": "normal" },
    "schemaVersion": 1,
    "updatedAt": "2026-11-06T12:01:00+00:00"
  },
  "user": {
    "id": "52771fbc-9fe0-4b9d-bcb1-38dfce181624",
    "level": "User",
    "data": { "pageSize": 25, "density": "compact" },
    "schemaVersion": 1,
    "updatedAt": "2026-11-06T12:00:00+00:00"
  }
}
```

Obě úrovně jsou oddělené, aby FE mohl zvolit vlastní prioritu nebo sloučení.
Jiný uživatel dostane stejné `admin`, ale pouze své `user`; pokud vlastní
nastavení nemá, dostane `user: null`.

Například FE může vybrat celou USER konfiguraci, jinak ADMIN a nakonec výchozí
nastavení z kódu:

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

Tento příklad vybírá celý JSON, neslučuje jednotlivé vlastnosti. Interpretace
`schemaVersion` a případná migrace obsahu jsou také odpovědností FE.

## Načtení více konfigurací

Při otevření stránky lze načíst více nastavení jedním požadavkem:

```http
POST /api/v2/component-configurations/query
Authorization: Bearer <token>
Content-Type: application/json

{
  "configurations": [
    { "componentId": "users-list", "configurationType": "export" },
    { "componentId": "dashboard-main", "configurationType": "widgets" }
  ]
}
```

Pokud pro tyto klíče zatím nic není uloženo, odpověď je `200 OK`:

```json
[
  {
    "componentId": "users-list",
    "configurationType": "export",
    "admin": null,
    "user": null
  },
  {
    "componentId": "dashboard-main",
    "configurationType": "widgets",
    "admin": null,
    "user": null
  }
]
```

Existující úrovně mají stejnou strukturu jako v GET. Odpověď zachovává pořadí
i opakované klíče; prázdné `configurations: []` vrátí `[]`. API počet klíčů
neomezuje. Typy `table`, `export` a `widgets` jsou nezávislá nastavení.

Chybějící konfigurace není chyba: i samostatný GET vrací `200` s příslušnou
úrovní `null`, aby FE mohl použít výchozí nastavení. Naproti tomu objekt
konfigurace s `data: null` znamená, že byla explicitně uložena JSON hodnota `null`.

## Návrat ke společnému nastavení

```http
DELETE /api/v2/component-configurations/users-list/table/user
Authorization: Bearer <token>
```

Odpověď `204 No Content` nemá tělo. Následující GET vrátí `user: null` a nezměněné
`admin`, takže FE může znovu použít společné nastavení. Pokud USER konfigurace
už neexistuje, DELETE vrátí `404 Not Found`.

Administrátor může obdobně smazat `/users-list/table/admin`; vlastní USER
nastavení tím nezmizí. Mazání je soft delete, nikoli fyzické odstranění JSONu
z databáze. Další PUT vytvoří konfiguraci s novým `id`.

## Pravidla a chyby

| Situace | Výsledek |
| --- | --- |
| Chybějící nebo neplatné přihlášení | `401 Unauthorized` |
| Chybějící oprávnění nebo stabilní identifikátor uživatele | `403 Forbidden` |
| Neplatný JSON, chybějící `data`, nekladná verze nebo neplatný klíč/dávka | `400 Bad Request` |
| Souběžná změna záznamu během zápisu nebo mazání | `409 Conflict` |
| Čtení neuložené konfigurace | `200 OK`, chybějící úrovně jsou `null` |
| Smazání neuložené konfigurace | `404 Not Found` |

- `componentId` má 1 až 200 znaků, `configurationType` 1 až 100. Klíče rozlišují
  velikost písmen a nesmějí mít mezery na začátku ani konci.
- `data` přijímá libovolnou JSON hodnotu, nejen objekt. Posílá se přímo jako JSON,
  ne jako escapovaný řetězec.
- `Context` odděluje konfigurace aplikací ve společné databázi. Nastavuje ho server,
  ne klient; DemoApi používá `DemoApi`. Nejde automaticky o bezpečné oddělení tenantů.
- DemoApi vrací `level` jako řetězec `Admin` nebo `User`; v jiném hostiteli záleží
  na nastavení JSON serializace.
- Při konfliktu `rowversion` server zápis neopakuje ani nepřepisuje souběžnou změnu;
  klient dostane `409 Conflict` a může znovu načíst aktuální konfiguraci. Verze
  není posílána klientovi, takže ochrana se týká souběžných zápisů v databázi,
  nikoli úprav založených na dávno načteném stavu na klientovi.

## Zapojení na backendu

Hostitel zaregistruje `AddConfigurationStorage<AppDbContext>()`, nastaví `Context`
a poskytne autentizaci s `IUserContext`. Výchozí identifikátor uživatele pochází
z `ClaimTypes.NameIdentifier`; pro nemapované JWT lze nastavit `UserIdClaimType = "sub"`.
Kontext dědí z `EntityFrameworkDbContextBase`, zpřístupní
`DbSet<ComponentConfiguration>` a zavolá `modelBuilder.AddConfigurationStorage()`
před `base.OnModelCreating(modelBuilder)`. HTTP API poskytuje controller dědící
z `ConfigurationStorageControllerBase`; hostitel mu určuje route a API verzi.
Před použitím je potřeba aplikovat EF migrace hostitelské aplikace.

Referenční zapojení je v DemoApi:
[registrace služeb](../../../src/Verisoft.DemoApi/Verisoft.DemoApi.Host/InstallExtensions/InstallExtensions.cs)
a [DbContext](../../../src/Verisoft.DemoApi/Verisoft.DemoApi.Data.EF/Context/DemoApiDbContext.cs).
Demo endpoint `GET /api/Auth/token?email=<email>` vydává token s rolí `Admin`,
proto s ním nelze ověřit zamítnutí ADMIN operace běžnému uživateli.