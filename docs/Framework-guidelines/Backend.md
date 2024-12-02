# Struktura BE
Název složky je ve formátu Projekt.NázevMikroservisy nebo NázevMikroservisy

/ root složka BE aplikace, obsahující konfigurační soubory a Dockerfile a sln 
└── XX.YY.Application/ business vrstva aplikace, obsahující services, modely a mapování 
└── XX.YY.Contracts/ kontrakty, které nabízí API rozhraní 
└── XX.YY.Common/ datová vrstva aplikace, obsahuje entity, repositáře, specifikace (filtrovací) 
└── XX.YY.Data.EF/ napojení na DB pomocí EF, obsahuje migrace 
└── XX.YY.Host/ spouštěcí projekt aplikace a prezentační vrstava, obsahuje endpointy (controllery)
└── XX.YY.Client/ vygenerovaný API client, kterého si mohou jako nuget referencovat ostatní mikroservices

## Obecná pravidla
### Metody 
Veškeré metody musí mít výstižný název popisující funkcionalitu metody, ze které musí být patrné k čemu slouží. Toto vede k dodržování principu single responsibility kdy metoda dělá pouze jednu věc, dalším benefitem je že nevznikají dlouhé metody, které jsou nečitelné pro vývojáře kvůli své komplexitě. 
Příklady: 

``` csharp
public Task<Entity> GetByIdAsync(int id, CancellationToken cancellationToken = default) 
```

``` csharp
public Task AddAttributesAsync(IEnumerable<CreateColumnRequest> columnRequests)
```
 


``` csharp
public static AddOpenApiSpecification(this IServiceCollection services) 
```


### Proměnné 
Veškeré proměnné musí mít název, který popisuje jejich funkci. Proměnné jsou inicializovány pouze ve scope jím náležícím a jejich životnost je co nejkratší. To vede k přehlednosti použitých proměnných a nedochází ke zbytečně brzkému inicializování proměnných a jejich pře používaní pro více účelů.

