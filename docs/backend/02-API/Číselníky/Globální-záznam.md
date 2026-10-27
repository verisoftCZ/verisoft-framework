#Jednoduchý vs. komplexní číselník

> Tento dokument dříve popisoval tenant model číselníků, který v aktuálním
> kódu neexistuje. Číselníky nemají koncept „globálního“ vs. „lokálního“
> záznamu vázaného na tenanta – rozdíl mezi číselníky je jen v tom, zda mají
> doplňující vlastnosti (komplexní) nebo ne (jednoduchý).

- **Jednoduchý číselník** – typ dědící od `CodebookRecordType` bez vlastních
  veřejných properties navíc. V DB se u entity `CodebookRecordEntity<T>`
  neukládají žádné `AdditionalProperties` (typ bez vlastností).
- **Komplexní číselník** – typ dědící od `CodebookRecordType`, který definuje
  vlastní veřejné property (např. `IsoAlpha2`, `IsoAlpha3` u číselníku zemí).
  Tyto vlastnosti se v DB uloží jako owned type `AdditionalProperties` u
  `CodebookRecordEntity<T>`.

**Příklad komplexního číselníku Country**

``` json
{
  "stringValue": "Czechia",
  "description": "Czech Republic",
  "additionalProperties": {
    "isoAlpha2": "CZ",
    "isoAlpha3": "CZE",
    "isoNumeric3": "204"
  },
  "validFrom": "2024-01-01",
  "validTo": null,
  "translations": [
    { "languageCode": "cs", "value": "Česko" },
    { "languageCode": "en", "value": "Czechia" }
  ]
}
```

**Příklad jednoduchého číselníku CommunicationType**

``` json
{
  "stringValue": "Email",
  "description": "E-Mail",
  "additionalProperties": {},
  "translations": [
    { "languageCode": "en", "value": "E-Mail" }
  ]
}
```

Platnost záznamu se řídí polemi `ValidFrom`/`ValidTo` (odvozená vlastnost
`IsValid`) – místo dřívějšího konceptu „forbidden pro tenanta“ se tímto řeší,
zda je hodnota aktuálně nabízena k výběru. Historické záznamy s ukončenou
platností zůstávají v DB dostupné (např. pro zobrazení existujících dat), ale
přes standardní filtr se nenabízejí jako nové volby.
