#Základní informace

Pro správu a používání číselníků používáme CodebookApi.

- Každý číselník má vlastní databázovou tabulku
- Každý číselník má vlastní controller
- Implementace nového číselníku je jednoduchá
- Číselník může být jednoduchý (pouze key/value) nebo komplexní (key/value + další vlastnosti)
- Číselníky podporují multitenant prostředí
- Klíčem číselníkové hodnoty (Record) je "StringValue"
- Jedna StringValue může být v tabulce vícekrát, pokud má některý tenant rozdílné hodnoty dalších vlastností (příklad 1)
- Číselníky podporují překlady, které nejsou závislé na tenantovi
- Record může být pro některého tenanta "Forbidden" a v tu chvíli by se neměl pro daného tenanta již nabízet
- Delete pouze nastavuje Record na isForbidden pro všechny tenanty

Příklad 1: 

Číselník sazeb DPH

|StringValue|TenantId|Description|VatRate|
|--|--|--|--|
|Standard|1 _(CZ)_|Standard VAT rate|21|
|Standard|2 _(SK)_|Standard VAT rate|20|

#Struktura contractu a význam jednotlivých propert



Příklad Country [globální záznam](/Framework-guidelines/Backend/Číselníky/Globální-záznam)
``` json
{
  "stringValue": "Czechia",                    // identifikátor číselníkové hodnoty (hodnota Enumu)
  "description": "Czech Republic",             // popis
  "defaultAdditionalProperties": {             // další vlastnosti komplexní číselníkové hodnoty pro defaultní záznam (pro tenanty, kteří nemají specifické hodnoty - záznam který je vytvořen s těmito defaultAdditionalProperties může být vázán na více tenantů)
    "isoAlpha2": "CZ",                         // ISO kód země ve formátu alpha 2
    "isoAlpha3": "CZE",                        // ISO kód země ve formátu alpha 3
    "isoNumeric3": "204"                       // ISO kód země ve formátu num 3
  },
  "tenantValues": [                            // kolekce specifických hodnot pro jednotlivé tenanty (dále upřesněno v článcích globální záznam a lokální záznam), tento konkrétní záznam znamená, že pro tenanta 5 je tato číselníková hodnota zakázána a tak by se neměla hodnota pro Českou republiku nabízet
    {
      "isoAlpha2": "CZ",
      "isoAlpha3": "CZE",
      "isoNumeric3": "204",
      "isForbidden": true,                     // označení, zda je pro daného tenanta tato číselníková hodnota (Czechia) zakázána
      "tenantId": 5,                           // id tenanta
    }
  ],
  "translations": [                            // kolekce překladů
    {
      "languageId": 1,                         // id jazyka (1 je default language, který musí být vždy nastaven - fallback, pokud neexistuje překlad pro požadovaný jazyk)
      "translationValue": "Czech Republic",    // hodnota překladu
    },
    {
      "languageId": 2,                         
      "translationValue": "Česká republika",   
    },
    {
      "languageId": 3,                         
      "translationValue": "Czechia",   
    }
  ],
}
```

