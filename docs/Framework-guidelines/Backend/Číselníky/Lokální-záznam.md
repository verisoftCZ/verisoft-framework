#Lokální hodnota
- Tato hodnota (Record) je přístupná pouze pro jednoho tenanta
- Aby byla hodnota vyhodnocena jako lokální musí v POST metodě přijít v "defaultAdditionalProperties" null a v tenant values musí být právě jeden element
- Slouží k tomu, aby v případě záznamu, který je určitě lokální nebylo potřeba zakazovat všem ostatním tenantům daný záznam

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
  "stringValue": "WinterWonderland",
  "description": "Place of miracles",
  "defaultAdditionalProperties": null,
  "tenantValues": [
    {
      "isoAlpha2": "WW",
      "isoAlpha3": "WWO",
      "isoNumeric3": "007",
      "visaRequired": false,
      "isForbidden": false,
      "tenantId": 1, // Česko
    }
  ],
  "translations": [
    {
      "languageId": 1,
      "translationValue": "Winter Wonderland",
    },
    {
      "languageId": 2,
      "translationValue": "Zimní říše divů",
    },
    {
      "languageId": 4,
      "translationValue": "Winter Wonderland",
    }
  ],
}
```
Výše popsaný request by založil takovýto záznam:

Pro tenanta 1 by byl záznam WinterWonderland povolený a additional properties by měly nastavenou hodnotu dle tenant values.
Tenanti 2, 3, 4 by záznam neviděli.

Podle toho jaký jazyk bude český uživatel požadovat se zobrazí buď Zimní říše divů (čeština) nebo  Winter Wonderland (vše ostatní).

**Číselník ComunicationType - jednoduchý**
``` js
{
  "stringValue": "Pigeon",
  "description": "Holub",
  "defaultAdditionalProperties": null,
  "tenantValues": [
    {
      "isForbidden": false,
      "tenantId": 1,
    }
  ],
  "translations": [
    {
      "languageId": 1,
      "translationValue": "Poštovní holub",
    }
  ],
}
```
Výše popsaný request by založil jeden záznam:

Pigeon se zobrazuje pouze pro tenanta 1 a má jen jediný překlad, který se zobrazí bez ohledu na požadovaný jazyk.