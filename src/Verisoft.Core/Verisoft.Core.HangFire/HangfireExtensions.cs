using Hangfire;
using Hangfire.PostgreSql;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Verisoft.Core.HangFire;
using Verisoft.Core.HangFire.Config;
using Verisoft.Core.HangFire.Filter;

namespace Verisoft.Core.HangFire;

public static class HangfireExtensions
{
    private const int WorkerCount = 50;

    public static void AddHangfire(this IServiceCollection serviceCollection, HangfireConfig hangfireConfig)
    {
        serviceCollection.AddSingleton<VerisoftHangfireLogProvider>();
        GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = hangfireConfig.NumberOfAutomaticRetries, OnAttemptsExceeded = AttemptsExceededAction.Fail });
        serviceCollection.AddHangfireServer(c =>
        {
            c.WorkerCount = WorkerCount;
            if (hangfireConfig.CustomProcessingQueues.Length != 0)
            {
                c.Queues = hangfireConfig.CustomProcessingQueues;
            }
        });

        serviceCollection.AddHangfire((sp, options ) =>
        {
            var logger = sp.GetService<VerisoftHangfireLogProvider>();
            var o = options.UseLogProvider(logger);
            if (hangfireConfig.UsePostgre)
            {
                UsePostgreSqlStorage(hangfireConfig, o);
            }
            else
            {
                UseMsSqlServerStorage(hangfireConfig, o);
            }
        });
    }

    public static void AddHangfire(this IServiceCollection serviceCollection, Action<HangfireConfig> options)
    {
        var config = new HangfireConfig();
        options?.Invoke(config);
        serviceCollection.AddHangfire(config);
    }

    public static IApplicationBuilder UseVerisoftHangfire(this IApplicationBuilder app)
    {
        var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();
        var hfOptions = new HangfireDashboardConfig(configuration);
        app.UseEndpoints(endpoints => endpoints.MapHangfireDashboard(options: new DashboardOptions
        {
            AppPath = hfOptions.AppPath,
            PrefixPath = hfOptions.PrefixPath,
            Authorization = hfOptions.AuthorizationEnabled
                ? [new AllowAccesFilter(hfOptions.AllowedRoles)]
                : [],
        }));

        return app;
    }

    private static void UsePostgreSqlStorage(HangfireConfig hangfireConfig, IGlobalConfiguration options)
    {
        var builder = new NpgsqlConnectionStringBuilder(hangfireConfig.ConnectionString)
        {
            SearchPath = hangfireConfig.HangfireSchemaName,
        };

        options.UsePostgreSqlStorage(config => config.UseNpgsqlConnection(builder.ConnectionString));
    }

    private static void UseMsSqlServerStorage(HangfireConfig hangfireConfig, IGlobalConfiguration options)
    {
        if (string.IsNullOrEmpty(hangfireConfig.HangfireSchemaName))
        {
            options.UseSqlServerStorage(hangfireConfig.ConnectionString);
        }
        else
        {
            options.UseSqlServerStorage(hangfireConfig.ConnectionString, new SqlServerStorageOptions()
            {
                SchemaName = hangfireConfig.HangfireSchemaName,
            });
        }
    }
}
