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

``` csharp
using Verisoft.Core.ApplicationInsights;

private static void RegisterMonitoring(IServiceCollection serviceCollection, IConfiguration configuration)
{
    serviceCollection.AddVerisoftApplicationInsights(configuration, new Verisoft.Core.ApplicationInsights.AIOptions());
}
```

Pro trackování vlastních eventů je možné použít `IBusinessTracker` z knihovny
`Verisoft.Core.ApplicationInsights`.

Příklad logování:
``` csharp

        logger.LogDebug("Getting client with ID {ClientId}", id);
        logger.LogInformation("Processing request to get client with ID {ClientId}", id);
        logger.LogWarning("Ensure that the client ID {ClientId} is valid", id);
        logger.LogError("An error occurred while trying to get client with ID {ClientId}", id);
        logger.LogCritical("Critical issue encountered when accessing client with ID {ClientId}", id);
        logger.LogTrace("Trace log for getting client with ID {ClientId}", id);
        businessTracker.TrackEvent("GetClient", new Dictionary<string, string> { { "ClientId", id.ToString() } });
        businessTracker.TrackMetric("GetClientCalls", 1);

        try
        {
            return await GetAsync(id);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Exception occurred while getting client with ID {ClientId}", id);
            logger.LogCritical(e, "Critical exception occurred while ge tting client with ID {ClientId}", id);
            businessTracker.TrackException(e, $"Error getting client with ID {id}");
            throw;
        }
```

# SQL trackování
Pokud potřebujete trackovat SQL dotazy do DB, stačí mít zapnuté (defaultně je) depedency tracking.
Ovšem appinsight neumožňují trackovat hodnoty SQL parametrů, takže je nutné trackovat buď přes console logy u sebe na stroji a nebo tyto logy pushovat do appinsights. Pozor tyto logy pak nebudou logovany jako dependency ale jako Trace.

Jak na to:
``` csharp
public class DemoApiDbContext(IDatabaseConfig databaseConfig, ILoggerFactory loggerFactory) : EntityFrameworkDbContextBase, IDemoApiDbContext
{
    //My DB context
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
                databaseConfig.ConnectionString,
                x => x.MigrationsHistoryTable("__MigrationsHistory", databaseConfig.DefaultSchema))
                .UseLoggerFactory(loggerFactory)  //tohle mi začne logovat do ILoggeru, tedy do trace
                .EnableSensitiveDataLogging();   //tohle mi zapne logování hodnot parametrů !!! nezapínat pro PROD
        }
    }
}
```
Nezapomeňte si také nastavit správný log level
``` js
"Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.EntityFrameworkCore.Database.Command": "Information"
    }
  }
```


# Health checks

registrace healthchecks do aplikace


``` csharp
private static void RegisterHealthChecks(IServiceCollection serviceCollection, IConfiguration configuration)
{
    serviceCollection.AddVerisoftHealth(c =>
    {
        c.ConnectionString = configuration["Database:ConnectionString"];  //connection string k DB
        c.DatabaseName = "<DBNAME>";
        c.Provider = HealthDatabaseProvider.SqlServer;                    //nebo HealthDatabaseProvider.PostgreSql
    });
}
```

v program.cs
`app.MapHealthChecks("/health");`

Test:
`http://localhost/health` v Docker Compose nebo port podle `launchSettings.json`
 při lokálním spuštění.


## Azure Monitor
Na tyto health checky je pak nutné napojit azure monitor aby sledoval jestli jsou mikroservisy dostupné a případně lze nastavit alerty na správce aplikace