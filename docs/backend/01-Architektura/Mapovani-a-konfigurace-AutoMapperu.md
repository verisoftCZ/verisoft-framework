# Mapování a AutoMapper

Framework nepoužívá AutoMapper (`IMapper`) napřímo v Application/Host vrstvě
– business kód pracuje s abstrakcí `ITypeMapper`
(`Verisoft.Core.Common.TypeMapper`), jejíž implementace
(`Verisoft.Core.TypeMapper.TypeMapper`) interně deleguje na AutoMapper:

```csharp
public class TypeMapper : ITypeMapper
{
    public TTarget Map<TSource, TTarget>(TSource source) => mapper.Map<TSource, TTarget>(source);

    public TTarget Map<TSource, TTarget>(TSource source, TTarget target) => mapper.Map(source, target);

    public TTarget Map<TTarget>(object source) => mapper.Map<TTarget>(source);

    public IEnumerable<TTarget> Map<TTarget>(IEnumerable<object> source) => mapper.Map<IEnumerable<TTarget>>(source);
}
```

Díky tomu je business logika (services) na AutoMapperu přímo nezávislá –
`ITypeMapper` lze v testech snadno mockovat a teoreticky by šlo mapovací
knihovnu vyměnit bez zásahu do `Services`.

## Konfigurace mapování (`AutoMapperFactory`)

Mapovací profily se v Demo API neregistrují přes `Profile` třídy s
auto-discovery (`AddAutoMapper(Assembly)`), ale explicitně ve statické
třídě `AutoMapperFactory` (`Verisoft.DemoApi.Application.Helpers`):

```csharp
public static class AutoMapperFactory
{
    public static IMapper CreateMapper()
    {
        MapperConfiguration config = CreateConfig();
        return config.CreateMapper();
    }

    private static MapperConfiguration CreateConfig()
    {
        return new MapperConfiguration(cfg =>
        {
            MapClient(cfg);
            MapUser(cfg);
            MapDocument(cfg);
            MapCodebookParent(cfg);
        });
    }

    private static void MapClient(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<ClientFilter, ClientSpecification>();
        cfg.CreateMap<ClientEntity, Client>();
        cfg.CreateMap<ClientEntity, ClientListItem>();
        cfg.CreateMap<ClientEditModel, ClientEntity>();
        cfg.CreateMap<ClientEditModel, ClientValidatedObject>();
    }

    // MapUser, MapDocument, MapCodebookParent obdobně...
}
```

Pro každou entitu se typicky definují tyto mapy (viz vzor DTO v
[Struktura backendu](Backend.md) – `{Entity}`, `{Entity}EditModel`,
`{Entity}ListItem`):

- `{Entity}Filter` → `{Entity}Specification` – pro převod API filtru na
  specifikaci použitou v repozitáři (viz [Užití](../03-Data/Databaze/Užití.md)).
- `{Entity}Entity` → `{Entity}` – entita na plné DTO (detail).
- `{Entity}Entity` → `{Entity}ListItem` – entita na položku seznamu.
- `{Entity}EditModel` → `{Entity}Entity` – vstupní model na entitu při
  vytvoření/úpravě (používá `ReadService`/`CrudService` interně přes
  `AddMapAsync`/`UpdateMapAsync`, viz
  [Aplikační vrstva a base služby](Aplikacni-vrstva-a-base-services.md)).
- `{Entity}EditModel` → `{Entity}ValidatedObject` – vstupní model na objekt
  používaný FluentValidation validátorem (viz [Validace](../02-API/Validace.md)).

`AutoMapperFactory.CreateMapper()` se zavolá jednou při startu a výsledný
`IMapper` i `ITypeMapper` se zaregistrují jako služby v `InstallExtensions`:

```csharp
private static void RegisterMapper(IServiceCollection serviceCollection)
{
    IMapper mapper = AutoMapperFactory.CreateMapper();
    serviceCollection.AddSingleton(mapper);
    serviceCollection.AddScoped<ITypeMapper, TypeMapper>();
}
```

## Přidání mapování pro novou entitu

1. V `AutoMapperFactory` přidejte novou privátní metodu `Map{Entity}` se
   všemi potřebnými `cfg.CreateMap<TSource, TTarget>()` a zavolejte ji z
   `CreateConfig()`.
2. AutoMapper defaultně mapuje property se stejným názvem automaticky;
   vlastní mapování property (`ForMember`) přidejte jen tam, kde se názvy
   liší nebo je potřeba transformace.
3. Pokud entitu/DTO tvoří i vnořené kolekce, ověřte, že AutoMapper najde i
   mapu pro vnořený typ (např. `Document` v rámci `Client`), jinak mapování
   vyhodí `AutoMapperMappingException` až za běhu (mapy se validují až při
   prvním použití, ne při startu).
4. Modul `Verisoft.Core.Codebooks.Common` má vlastní
   `CodebookMappingProfile` (klasický AutoMapper `Profile`) pro mapování
   číselníkových DTO – u vlastních číselníků se řiďte tímto vzorem, pokud
   přidáváte netriviální mapování, viz [Číselníky](../02-API/Číselníky.md).
