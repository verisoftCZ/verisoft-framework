using System;
using System.IO;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Verisoft.Core.AspNet.Filters;

namespace Verisoft.Core.AspNet.OpenApi;

public static class InstallExtensions
{
    public static void AddVerisoftOpenApiSpecification(
        this IServiceCollection services,
        Action<ApiVersioningOptions> apiVersioningOptions = null,
        Action<ApiExplorerOptions> apiExplorerOptions = null,
        Action<SwaggerGenOptions> swaggerOptions = null,
        Action<OpenApiInfo> apiInfo = null)
    {
        AddVerisoftApiVersioning(services, apiVersioningOptions, apiExplorerOptions);
        AddVerisoftSwaggerGen(services, swaggerOptions, apiInfo);
    }

    public static void AddVerisoftApiVersioning(
        this IServiceCollection services,
        Action<ApiVersioningOptions> apiVersioningOptions = null,
        Action<ApiExplorerOptions> apiExplorerOptions = null)
    {
        var apiVersioningBuilder = services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
            options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader());

            apiVersioningOptions?.Invoke(options);
        });

        apiVersioningBuilder.AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;

            apiExplorerOptions?.Invoke(options);
        });
    }

    public static void AddVerisoftSwaggerGen(
    this IServiceCollection services,
    Action<SwaggerGenOptions> swaggerOptions = null,
    Action<OpenApiInfo> apiInfo = null)
    {
        services.AddSwaggerGen(options =>
        {
            options.OperationFilter<SwaggerDefaultValues>();
            options.SchemaFilter<ExcludeDeclaredTypePropertySchemaFilter>();

            var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath);
            }

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Bearer JWT Token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer",
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer",
                        },
                    },
                    Array.Empty<string>()
                },
            });

            swaggerOptions?.Invoke(options);
        });

        services.AddTransient<IConfigureOptions<SwaggerGenOptions>>(sp =>
        {
            var provider = sp.GetRequiredService<IApiVersionDescriptionProvider>();
            return new ConfigureSwaggerOptions(provider, apiInfo);
        });
    }
}
