#Základní informace

Číselníky (codebooks) jsou implementovány v `Verisoft.Core.Codebooks` (služby,
minimal API endpointy) a `Verisoft.Core.Codebooks.Common` (kontrakty, entity,
EF konfigurace, DTO). Konzumující API (např. `Verisoft.DemoApi.Host`)
registruje konkrétní typy číselníků přes `AddCodebooksForConsumer()`; samostatný
`Verisoft.CodebookApi.Host` místo toho volá `AddCodebooks()`.

- Každý číselník je reprezentován vlastní třídou dědící od `CodebookRecordType`
  (abstraktní značkovací třída ve `Verisoft.Core.Common.Entities.Codebooks`),
  např. `Accessibility` v DemoApi.
- Perzistence používá generickou entitu `CodebookRecordEntity<T>`; tabulka a
  route jsou odvozené z názvu typu a prefixů (`CodebookRecordTablePrefix`,
  `CodebookRecordRoutePrefix`) nastavených při registraci v `CodebookRegistry`.
- Klíčem číselníkové hodnoty je `StringValue` (alternativní klíč v DB).
- **Jednoduchý číselník** nemá žádné vlastní properties navíc – typ dědící od
  `CodebookRecordType` obsahuje jen statické instance (`CodebookRecord<T>`)
  reprezentující jednotlivé hodnoty.
- **Komplexní číselník** definuje na typu vlastní veřejné property navíc, které
  se v DB uloží jako owned type `AdditionalProperties` u `CodebookRecordEntity<T>`.
- Číselníky podporují platnost v čase (`ValidFrom`, `ValidTo`) a odvozenou
  vlastnost `IsValid`.
- Překlady jsou samostatná entita `CodebookRecordTranslationEntity<T>` vázaná
  na konkrétní záznam, s `LanguageCode` (řetězec, ne numerické ID jazyka) a
  hodnotou překladu.
- **Číselníky aktuálně nejsou multitenantní.** V entitách, EF konfiguraci
  (`CodebookRecordConfiguration`) ani kontraktech (`StringValueDetail`,
  `StringValueDetailEditModel`, `StringValueDetailCreateModel`) není žádný
  koncept tenanta, `TenantId` ani zamykání/„forbidden“ hodnot na úrovni
  tenanta.
- Repozitáře, služby (`ICodebookRecordService<T>`, `IStringValueService<T>`) a
  endpointy pro každý registrovaný typ vznikají automaticky přes
  `MainCodebookRecordConfigurationBuilder` a
  `WebApplicationExtensions.ConfigureEndpointsV1<TCodebookRecordType>()` –
  není potřeba psát vlastní controller pro každý číselník. Lze ale
  zaregistrovat vlastní implementaci repository/service nebo přidat vlastní
  endpoint (`CodebookRecordConfiguration.CustomEndpoints`).

#Struktura kontraktu a význam jednotlivých properties

Příklad detailu hodnoty (`StringValueDetail<TAdditionalProperties>`):

``` json
{
  "id": 12,
  "stringValue": "Public",             // identifikátor číselníkové hodnoty (odpovídá statické property na typu CodebookRecordType, např. Accessibility.Public)
  "description": "Public accessibility", // volitelný popis záznamu
  "additionalProperties": {},           // doplňující vlastnosti komplexního číselníku; u jednoduchého číselníku bez veřejných properties (prázdný objekt)
  "validFrom": "2024-01-01",
  "validTo": null,
  "isValid": true                      // odvozeno z aktuálního data a validFrom/validTo
}
```

Překlady se řeší samostatně přes `UpsertTranslationAsync` /
`RemoveTranslationAsync` na `IStringValueService<TCodebookRecordType>` a jsou
vázané na `languageCode`, nikoli na numerické ID jazyka nebo tenanta.
