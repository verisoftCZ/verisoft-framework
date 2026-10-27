#Překlady číselníkových hodnot

> Tento dokument dříve popisoval "lokální hodnotu" vázanou na jednoho
> tenanta. Číselníky nejsou multitenantní, tento koncept v kódu neexistuje a
> byl nahrazen popisem skutečného mechanismu překladů.

- Překlad je samostatná entita `CodebookRecordTranslationEntity<T>` vázaná na
  konkrétní `StringValue` číselníkové hodnoty.
- Klíčem překladu je dvojice `StringValue` + `LanguageCode` (řetězcový kód
  jazyka, např. `cs`, `en` – ne numerické ID jazyka).
- Překlad se přidává/upravuje přes
  `IStringValueService<T>.UpsertTranslationAsync(stringValue, languageCode, value)`
  a odstraňuje přes `RemoveTranslationAsync(stringValue, languageCode)`.
- Pokud pro požadovaný `languageCode` překlad neexistuje, endpointy vracející
  jeden záznam (`GET .../{NázevČíselníku}`, `GET .../{NázevČíselníku}/{stringValue}`)
  použijí `LanguageConstants.DefaultLanguageCode` jako fallback.

**Příklad**

``` json
{
  "stringValue": "Czechia",
  "translations": [
    { "languageCode": "cs", "value": "Česko" },
    { "languageCode": "en", "value": "Czechia" },
    { "languageCode": "de", "value": "Tschechien" }
  ]
}
```

Detail záznamu (`GET .../{NázevČíselníku}/{stringValue}/Detail`) vrací vždy
všechny překlady najednou; endpointy pro seznam/jeden záznam vrací jen jeden
překlad podle požadovaného jazyka.
