#Serializace

``` csharp
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());      // 1
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;   // 2
});
```
1. Zajistí, že se Enumy z controlleru vrací ve své podobě .ToString() a také je lze jako string posílat coby vstupní hodnoty.
2. Při serializaci do .json jsou ignorovány cykly. Tedy například, pokud Customer obsahuje třídu Address, která opět obsahuje třídu Customer, není již Customer podruhé v .json vypsán jako podřízený objekt objektu Address. Zamezí padání rozhraní z důvodu moc hlubokého zanoření (max 32 úrovní)

#Filtry
``` csharp
serviceCollection.AddSwaggerGen(options =>
{
     options.OperationFilter<Core.AspNet.Filters.SwaggerDefaultValues>();                     // 3
     options.SchemaFilter<Core.AspNet.Filters.NullableSchemaObjectFilter>("Verisoft");        // 4
}
```
3. Nezbytný filtr pro správné sestavení swagger.json podporujícím verzování
4. Explicitně nastaví složité datové type, které jsou z namespace začínajícím na "Verisoft" jako nullable. NSwag při generování klienta počítá s tím, že property a parametry které nejsou explicitně nullable tak jsou non-nullable, což je v rozporu s nullabilitou referenčních typů.

Filtry lze také přidat přes `Verisoft.Core.AspNet.OpenApi.AddVerisoftSwaggerGen()` metodu, do kterého mohu vložit další custom filtry pomocí parametru swaggerOptions.


``` csharp
serviceCollection.AddVerisoftSwaggerGen(swaggerOptions: options =>
{
    options.SchemaFilter<NullableObjectSchemaFilter>("Verisoft");
});
```
**Rovněž lze použít metodu `Verisoft.Core.AspNet.OpenApi.AddVerisoftOpenApiSpecification()` se stejným parametrem jako metoda výše. (preferované řešení)**
``` csharp
serviceCollection.AddVerisoftOpenApiSpecification(
    apiVersioningOptions: options =>
    {
        options.DefaultApiVersion = new ApiVersion(2, 0);
    },
    apiExplorerOptions: options =>
    {
        options.GroupNameFormat = "'v'V";
    },
    swaggerOptions: options =>
    {
        options.SchemaFilter<NullableObjectSchemaFilter>("Verisoft");
    },
    apiInfo: info =>
    {
        info.Title = "Verisoft Demo.API";
        info.Description = "Verisoft demo API for showcase purposes";
        info.Contact = new OpenApiContact { Name = "Jack Szabo", Email = "jack.szabo@verisoft.cz" };
        info.License = new OpenApiLicense { Name = "MIT", Url = new Uri(MitLicenceUrl) };
    });
```
