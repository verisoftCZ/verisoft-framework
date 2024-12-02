using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Verisoft.Core.AspNet.Hangfire;

namespace Verisoft.Core.AspNet
{
    public static class HangfireExtensions
    {
        private const int WorkerCount = 50;

        public static void AddHangfire(this IServiceCollection serviceCollection, HangfireConfig hangfireConfig)
        {
            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = hangfireConfig.NumberOfAutomaticRetries, OnAttemptsExceeded = AttemptsExceededAction.Fail });
            serviceCollection.AddHangfireServer(c =>
            {
                c.WorkerCount = WorkerCount;
                if (hangfireConfig.CustomProcessingQueues.Any())
                {
                    c.Queues = hangfireConfig.CustomProcessingQueues;
                }
            });

            serviceCollection.AddHangfire(options =>
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
            });
        }

        public static void AddHangfire(this IServiceCollection serviceCollection, Action<HangfireConfig> options)
        {
            var config = new HangfireConfig();
            options?.Invoke(config);
            AddHangfire(serviceCollection, config);
        }

        public static void UseHangfire(this IApplicationBuilder app)
        {
            app.UseHangfireDashboard(options: new DashboardOptions
            {
                Authorization = new[] { new AllowAccesFilter(new List<string>() { "Admin" }) },
            });
        }
    }
}
