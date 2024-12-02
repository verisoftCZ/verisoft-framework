#Globální hodnota
- Tato hodnota (Record) může být přístupná pro všechny tenanty, pokud se tak nastaví.
- Aby byla hodnota vyhodnocena jako globální musí v POST metodě přijít hodnota "defaultAdditionalProperties", i kdyby měla být prázdná {}
- V tenant values lze pro konkrétního tenanta hodnotu vyloučit nebo pro ni nastavit jiné additional properties, než jsou default
- Pokud je tenant value pro tenanta nastavena, je k tenantovi přistupováno jako k odchylce od defaultu bez ohledu na to, zda se hodnota additional properties liší
- Pro číselník, který nemá additional properties nelze založit tenant value jinou, než takovou, která má isForbidden = true


**Příklady**
_V našem prostředí existují 4 tenanti_
_1. Česko_
_2. Slovensko_
_3. Polsko_
_4. Indie_

_a 4 jazyky_

_1. Default_
_2. Čeština_
_3. Němčina_
_4. Angličtina_

**Číselník Country - komplexní**
``` js
{
  "stringValue": "Czechia",
  "description": "Czech Republic",
  "defaultAdditionalProperties": {
    "isoAlpha2": "CZ",
    "isoAlpha3": "CZE",
    "isoNumeric3": "204"
    "visaRequired": false
  },
  "tenantValues": [
    {
      "isoAlpha2": "CZ",
      "isoAlpha3": "CZE",
      "isoNumeric3": "204",
      "visaRequired": true,
      "isForbidden": false,
      "tenantId": 4, // Indie
    },
    {
      "isoAlpha2": "CZ",
      "isoAlpha3": "CZE",
      "isoNumeric3": "204",
      "visaRequired": false,
      "isForbidden": true,
      "tenantId": 3, // Polsko
    }
  ],
  "translations": [
    {
      "languageId": 1,
      "translationValue": "Czech Republic",
    },
    {
      "languageId": 2,
      "translationValue": "Česko",
    },
    {
      "languageId": 3,
      "translationValue": "Tschechische Republik",
    }
  ],
}
```


Výše popsaný request by založil takovýto záznam:

Pro tenanty 1 a 2 by byl záznam Czechia povolený a additional properties by měly nastavenou hodnotu dle default (bezvízový styk).
Tenant 3 by neměl záznam povolený (např. Polsko přestalo uznávat ČR jako svrchovaný stát).
Tenant 4 by měl záznam povolený, ale měl by pro tuto zemi evidováno, že s ní vede vízový styk.

Bez ohledu jaký tenant se na záznam ptá, překlady se vrací dle StringValue "Czechia" na základě požadovaného jazyka, pokud překlad neexistuje, vrací se Default language, tedy v tomto případě Czech Republic.

**Číselník ComunicationType - jednoduchý**


``` js
{
  "stringValue": "Fax",
  "description": "Fax",
  "defaultAdditionalProperties": {},
  "tenantValues": [
    {
      "isForbidden": true,
      "tenantId": 1,
    }
  ],
  "translations": [
    {
      "languageId": 1,
      "translationValue": "Fax",
    }
  ],
}
```
```js
{
  "stringValue": "Email",
  "description": "E-Mail",
  "defaultAdditionalProperties": {},
  "tenantValues": []
  "translations": [
    {
      "languageId": 1,
      "translationValue": "E-Mail",
    }
  ],
}
```
``` js
{
  "stringValue": "Phone",
  "description": "Phone",
  "defaultAdditionalProperties": {},
  "tenantValues": []
  "translations": [
    {
      "languageId": 1,
      "translationValue": "Phone",
    }
    {
      "languageId": 2,
      "translationValue": "Telefon",
    }
  ],
}
```
Výše popsané requesty by založily takovéto záznamy:

Je založen jeden defaultní záznam pro Fax, který je povolen všude s výjimkou ČR. Má jediný překlad.
Je založen jeden defaultní záznam pro Email, který je povolen všude. Má jediný překlad.
Je založen jeden defaultní záznam pro Phone, který je povolen všude. Má defaultní a český překlad.

#Bulk
- Je možné přidávat záznamy hromadně, přes endpoint /AddBulk
- Tyto záznamy jsou založeny jako globální s hodnotou forbidden = false pro všechny tenanty
- Přes tento endpoint lze nastavit překlad pouze pro Default language
``` js
{
  "stringValues": [
    {
      "stringValue": "Male",
      "defaultTranslation": "Male"
    },
    {
      "stringValue": "Female",
      "defaultTranslation": "Female"
    }
  ]
}
```
_Šlo by rozšířit o_
- _podporu více překladů_
- _podporu zakazování záznamů pro tenanty_