using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace Verisoft.Core.OpenApi.OpenApi
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder AddOpenApi(this IApplicationBuilder app, Action<VerisoftSwaggerOptions>? setupAction = null)
        {
            var veriOptions = new VerisoftSwaggerOptions();
            setupAction?.Invoke(veriOptions);

            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    if (veriOptions.Url != null)
                    {
                        swaggerDoc.Servers = [new OpenApiServer { Url = $"/{veriOptions.Url}" }];
                    }
                });
            });

            app.UseSwaggerUI(options =>
            {
                var descriptions = ((IEndpointRouteBuilder)app).DescribeApiVersions();
                var groupNames = descriptions.Select(x => x.GroupName);

                foreach (var description in groupNames)
                {
                    var url = $"{description}/swagger.json";
                    var name = description.ToUpperInvariant();
                    options.SwaggerEndpoint(url, name);
                }
            });
            return app;
        }
    }
}
