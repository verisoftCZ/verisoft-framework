Pro CRON joby používáme Hangfire.

Jak si zaregistrovat hangfire:
    
``` csharp
private static void RegisterHangfire(IServiceCollection serviceCollection, IConfiguration configuration)
{
    serviceCollection.AddHangfire(options =>
    {
        options.HangfireSchemaName = "Hangfire";
        options.ConnectionString = configuration["Database:ConnectionString"];
    });
    HangfireJobsConfig hangfireJobsConfig = new HangfireJobsConfig(configuration);
    serviceCollection.AddSingleton(hangfireJobsConfig);
}

///Je možné přenastavit všechny tyto hodnoty:

public class HangfireConfig
{
    public string HangfireSchemaName { get; set; } = null;

    public string[] CustomProcessingQueues { get; set; } = [];

    public string ConnectionString { get; set; } = string.Empty;

    public int NumberOfAutomaticRetries { get; set; } = 0;

    public bool UsePostgre { get; set; } = false;  //if you want postgre not mssql
}

```
a v Program.cs

``` csharp
app.UseVerisoftHangfire();
app.RegisterCronJobs();
```

Pro zaregistrování jobu:

``` csharp
public static class CronRegistrator
{
    public static void RegisterCronJobs(this IApplicationBuilder app)
    {
        var hangfireJobsConfig = app.ApplicationServices.GetService<HangfireJobsConfig>();

        CronJobSetup.AddCronJob<DemoCronJob>(
            (x) => x.MakeSomeCronJob(),
            options =>
            {
                options.CronEnabled = false;
                options.CronSettings = hangfireJobsConfig.JobOneSettings;
            });

        CronJobSetup.AddCronJob<DemoCronJob>(
           (x) => x.MakeSomeCronJobTwo(),
           options =>
           {
               options.CronEnabled = true;
               options.CronSettings = hangfireJobsConfig.JobOneSettings;
           });
    }
}
```
HangfireJobsConfig
``` csharp
public class HangfireJobsConfig
{
    public HangfireJobsConfig(IConfiguration configuration)
    {
        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        configuration.GetSection("HangfireJobs").Bind(this);
    }

    public string JobOneSettings { get; set; }

    public string JobTwoSettings { get; set; }
}
```

v appsettings:
  
``` js
"HangfireJobs": {
    "JobOneSettings": "0 0 * * *",
    "JobTwoSettings": "0 1 * * *"
  },
```

Dashboard je mozne konfigurovat pomocí:
``` csharp
public class HangfireDashboardConfig
{
    public HangfireDashboardConfig(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        configuration.GetSection("HangfireDashboardConfig").Bind(this);
    }

    public string? AppPath { get; set; } = null;

    public string? PrefixPath { get; set; } = null;

    public bool AuthorizationEnabled { get; set; } = true;

    public string[] AllowedRoles { get; set; } = ["Admin"];
}
````

Autorizace dashboardu je ve vychozim stavu zapnuta a vyzaduje jednu z roli
v `AllowedRoles` (standardne `Admin`). Pro dashboard bez autorizace nastavte
v appsettings:

``` json
{
  "HangfireDashboardConfig": {
    "AuthorizationEnabled": false
  }
}
```

Pri `AuthorizationEnabled: false` se `AllowedRoles` nekontroluji a dashboard
je pristupny i anonymnim uzivatelum, vcetne spravy jobu. Tuto moznost pouzivejte
pouze v duveryhodnem prostredi nebo s ochranou pristupu mimo aplikaci.
Prazdne `AllowedRoles` pri zapnute autorizaci nadale pristup zakazuji.
