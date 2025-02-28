using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Verisoft.Core.AspNet.Filters;
using Verisoft.Core.AspNet.OpenApi;
using Verisoft.Core.Common.Logging;
using Verisoft.Core.Common.Store;

namespace Verisoft.Core.AspNet
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseVerisoftCoreAspNet(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(a => a.Run(async context =>
            {
                await CommonExceptionHandler.HandleAsync(context);
            }));
        }

        public static void UseVerisoftAvailabilityTracking(this IApplicationBuilder app)
        {
            var tracker = app.ApplicationServices.GetService<IBusinessTracker>();
            tracker.StartSelfAvailabilityTest(CancellationToken.None);
        }

        public static IApplicationBuilder AddOpenApi(this IApplicationBuilder app, Action<VerisoftSwaggerOptions> setupAction = null)
        {
            VerisoftSwaggerOptions veriOptions = new VerisoftSwaggerOptions();
            setupAction?.Invoke(veriOptions);

            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    if (veriOptions.Url != null)
                    {
                        swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"https://{httpReq.Host.Value}/{veriOptions.Url}" } };
                    }
                });
            });
            app.UseSwaggerUI(options =>
            {
                var descriptions = ((WebApplication)app).DescribeApiVersions();
                var groupNames = descriptions.Select(x => x.GroupName);

                // Build a swagger endpoint for each discovered API version
                foreach (var description in groupNames)
                {
                    var url = veriOptions.Url != null ? $"{veriOptions.Url}/{description}/swagger.json" : $"{description}/swagger.json";
                    var name = description.ToUpperInvariant();
                    options.SwaggerEndpoint(url, name);
                }
            });
            return app;
        }

        public static void MigrateDatabase<TDBContext>(this IApplicationBuilder app)
            where TDBContext : IMigrable
        {
            var scopeFactory = app.ApplicationServices.GetService<IServiceScopeFactory>();
            if (scopeFactory == null)
            {
                return;
            }

            using var scope = scopeFactory.CreateScope();
            scope.ServiceProvider.GetRequiredService<TDBContext>().Migrate();
        }
    }
}
