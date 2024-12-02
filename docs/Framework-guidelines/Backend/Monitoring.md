# Aplikační monitoring

Aplikační monitoring řešíme pomocí Application Insights.
Konkrétně přes Microsoft.ApplicationIsnights.AspNetCore

1. krokem je vytvoření application insights v azure portál a vygenerování application insights connection stringu
2. krokem je přidání connection stringu do appsettings.ENV.json

``` js
{
  "ApplicationInsights": {
    "ConnectionString": "InstrumentationKey=YOUR_INSTRUMENTATION_KEY;IngestionEndpoint=https://YOUR_REGION_ENDPOINT"
  },
  // ... other settings
}
```

použití v projektu:
``` csharp
using Verisoft.Core.AspNet

private static void RegisterMonitoring(IServiceCollection serviceCollection, IConfiguration configuration)
{
    serviceCollection.AddVerisoftApplicationInsights(configuration, new Verisoft.Core.AspNet.ApplicationInsights.AIOptions()
    {
        EnableSqlCommandTextTracking = true,  //povolí/zakáže logovat SQL do appinsights
        EnableHangfireTracking = false,       //disables hangfire SQL dependencies, but allows tracking of other - nedoporučuji používat genruje spoustu eventu každou vteřinu a plní applicationsight což je drahé
    });
}
```

Pro trackování vlastních eventů je možné použít `IBusinessTracker` z nugetu `Verisoft.Core.AspNet` 
 konkrétně jde (Verisoft.Core.AspNet.ApplicationInsights)

# Healh checks

registrace healthchecks do aplikace


``` csharp
private static void RegisterHealthChecks(IServiceCollection serviceCollection, IConfiguration configuration)
{
    serviceCollection.AddVerisoftHealth(configuration, c =>
    {
        c.ConnectionString = configuration["Database:ConnectionString"];  //connection string k DB
        c.DatabaseName = "<DBNAME>";
    });
}
```

v program.cs
`app.MapHealthChecks("/health");`

Test:
https://localhost:9999/health  - nutné změnit port


## Azure Monitor
Na tyto health checky je pak nutné napojit azure monitor aby sledoval jestli jsou mikroservisy dostupné a případně lze nastavit alerty na správce aplikace