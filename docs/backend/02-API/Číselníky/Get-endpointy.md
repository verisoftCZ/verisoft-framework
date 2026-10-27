#Endpointy pro záznamy číselníku

Pro každý registrovaný číselník (`CodebookRecordType`) se automaticky generují
následující minimal-API endpointy (`WebApplicationExtensions.ConfigureEndpointsV1<T>`,
`Verisoft.Core.Codebooks`), pod prefixem `v{verze}/{RoutePrefix}/{NázevČíselníku}`:

**GET .../{NázevČíselníku}**
- Filtrovaná a stránkovaná data (`CodebookRecordFilter`: `Translation`,
  `LanguageCode`, `Exclude`, `StringValue`) – vrací kolekci `CodebookRecord<T>`
  s překladem podle `filter.languageCode` (default jazyk viz
  `LanguageConstants.DefaultLanguageCode`).

**GET .../{NázevČíselníku}/{stringValue}**
- Jeden záznam podle `stringValue`, volitelně s `languageCode` query
  parametrem pro konkrétní překlad.

**GET .../{NázevČíselníku}/{stringValue}/Detail**
- Detail záznamu (`StringValueDetail<T>`) – obsahuje i platnost
  (`ValidFrom`/`ValidTo`), `AdditionalProperties` a všechny překlady. Stejný
  kontrakt se používá i pro `POST`/`PUT`.

**GET .../{NázevČíselníku}/DetailList**
- Stránkovaná kolekce detailů (`StringValueDetailListData<T>`) filtrovaná přes
  `StringValueDetailFilter.SearchTerm`.

**GET .../{NázevČíselníku}/{stringValue}/IsAvailable**
- `bool`, zda daná `stringValue` u číselníku existuje a je platná – slouží
  k BE validaci vstupů (`ICodebookRecordService<T>.IsCodebookRecordAvailableAsync`).

**POST .../{NázevČíselníku}**
- Vytvoření nového záznamu, tělo `StringValueDetailCreateModel<T>`; před
  uložením proběhne validace přes `IStringValueService<T>.ValidateAsync`.

**PUT .../{NázevČíselníku}/{stringValue}**
- Úprava existujícího záznamu, tělo `StringValueDetailEditModel<T>`.

**DELETE .../{NázevČíselníku}/{stringValue}**
- Odstranění záznamu (`RemoveStringValueAsync`) – řídí se stejnou soft-delete
  logikou jako ostatní entity dědící z `BaseEntity` (viz [Databáze](../../03-Data/Databaze.md)).

**POST .../{NázevČíselníku}/{stringValue}/Translation/{languageCode}/{value}**
- Přidání/úprava překladu pro daný jazyk (`UpsertTranslationAsync`).

**DELETE .../{NázevČíselníku}/{stringValue}/Translation/{languageCode}**
- Odstranění překladu pro daný jazyk (`RemoveTranslationAsync`).

Kromě těchto standardních endpointů lze pro konkrétní číselník zaregistrovat i
vlastní (`CodebookRecordConfiguration.CustomEndpoints`), viz sekce
`CustomEndpoints` v `WebApplicationExtensions`.

> Číselníky nejsou multitenantní – žádný z uvedených endpointů nefiltruje
> podle tenanta ani nepracuje s konceptem „forbidden pro tenanta“. Přístup ke
> všem záznamům je řízen jen přes autorizaci (`RequirePermission` na základě
> `CodebookRecordRequiredPermissionConfiguration`).