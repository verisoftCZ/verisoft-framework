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
}

```
a v Program.cs

``` csharp
app.UseHangfire();
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
