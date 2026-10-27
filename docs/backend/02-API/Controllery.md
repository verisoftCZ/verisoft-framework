#Controllery

Backendové controllery (`Microsoft.AspNetCore.Mvc.ControllerBase`) jsou
umístěné v `Host/Controllers/V{verze}/` konzumujícího API (např.
`Verisoft.DemoApi.Host.Controllers.V2.ClientController`). Standardně dědí od
generických base controllerů `CrudController<>`/`BaseReadController<>`
(`Verisoft.DemoApi.Application.Controllers`), které poskytují opakovaně
použitelnou implementaci CRUD operací.

Konvence, kterými se controllery řídí, jsou popsané v podstránkách:

- [Verzování](Controllery/Verzování.md) – všechny controllery jsou verzované
  přes `Asp.Versioning`, preferovaně nastavené pomocí
  `AddVerisoftApiVersioning()`/`AddVerisoftOpenApiSpecification()`.
- [Endpointy a routing](Controllery/Endpointy-a-routing.md) – standardní
  controller obsahuje 5 základních endpointů (POST, PUT, DELETE, GET detail,
  GET list) s hierarchickým routingem pro další akce.
- [Návratové kódy a typy](Controllery/Návratové-kódy-a-typy.md) – společné
  HTTP stavové kódy se nastavují na úrovni controlleru přes
  `ProducesResponseType`, výsledek business logiky se do `IActionResult`
  převádí přes extension metodu `ToActionResult()` nad
  `BusinessActionResult<TResult>`.
- [Nastavení (filtry, serializace...)](<Controllery/Nastavení-(filtry,-serializace...).md>) –
  JSON serializace enumů jako stringů, `ReferenceHandler.IgnoreCycles` a
  Swagger/OpenAPI filtry (`SwaggerDefaultValues`, `NullableObjectSchemaFilter`)
  nastavované přes `AddVerisoftSwaggerGen()`/`AddVerisoftOpenApiSpecification()`.
- [Dokumentace](Controllery/Dokumentace.md) – XML komentáře nad veřejnými
  metodami se promítají do Swagger dokumentace; generování `.xml` souboru
  zajišťuje `AddVerisoftOpenApiSpecification()`.

Všechny výše zmíněné `AddVerisoft*` metody jsou definované ve třídě
`Verisoft.Core.OpenApi.OpenApi.InstallExtensions` a registrují se v
`InstallExtensions` hostitelské aplikace (`Host/InstallExtensions/InstallExtensions.cs`).

Business logiku, kterou controllery volají, popisuje
[Aplikační vrstva a base služby](../01-Architektura/Aplikacni-vrstva-a-base-services.md)
(base služby `ReadService`/`CrudService` a jejich hooky) a
[Mapování a konfigurace AutoMapperu](../01-Architektura/Mapovani-a-konfigurace-AutoMapperu.md). Kompletní
příklad propojení controller → service → repository na `Client` entitě je v
[Endpointy a routing](Controllery/Endpointy-a-routing.md).
