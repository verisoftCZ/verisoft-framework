#Verzování

Controllery vždy verzujeme a to za pomoci Asp.Versioning NuGet.
Pokud verzujeme závislost, lze ji injectnout pomocí atributu [FromKeyedServices(version)].
``` csharp
using Asp.Versioning;

namespace Verisoft.DemoApi.Host.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("v{v:apiVersion}/[controller]")]
public class DemoController([FromKeyedServices("v1")] IDemoService demoService)
{
    // ... endpoints
}
```
Registrace verzovaných service a repository probíhá pomocí extension metod např. TryAddKeyedScoped
``` csharp
private static void RegisterServices(IServiceCollection serviceCollection)
{
    serviceCollection.TryAddKeyedScoped<IDemoService, Application.Services.V1.DemoService>("v1");
    serviceCollection.TryAddKeyedScoped<IDemoService, Application.Services.V2.DemoService>("v2");
}
```
Registrace verzování do aplikace - použití v Program.cs
``` csharp
var apiVersioningBuilder = builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader());
});

apiVersioningBuilder.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});
```
``` csharp
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        // ... other OpenApiInfo
    });
    options.OperationFilter<SwaggerDefaultValues>();
    // ... other filters and options
});
```
``` csharp
app.UseSwaggerUI(options =>
{
    var descriptions = app.DescribeApiVersions();
    foreach (var description in descriptions)
    {
        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
    }
    // ... other options
});
```

**Preferované je přidat verzování pomocí metody `Verisoft.Core.AspNet.OpenApi.AddVerisoftOpenApiSpecification()` s možností parametrizovat jednotlivé options**


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

nebo metodu `Verisoft.Core.AspNet.OpenApi.AddVerisoftApiVersioning()`, která má jen parametry pro apiVersioningOptions a apiExplorerOptions