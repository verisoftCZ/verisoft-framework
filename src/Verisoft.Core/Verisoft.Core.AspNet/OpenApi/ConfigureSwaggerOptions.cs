using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace Verisoft.Core.AspNet.OpenApi;

public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, Action<OpenApiInfo> configureApiInfo = null) : IConfigureOptions<SwaggerGenOptions>
{
    private const string VerisoftUrl = "https://verisoft.cz";

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            var info = CreateInfoForApiVersion(description);

            configureApiInfo?.Invoke(info);

            options.SwaggerDoc(description.GroupName, info);
        }
    }

    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var info = new OpenApiInfo()
        {
            Title = "Verisoft API",
            Version = description.ApiVersion.ToString(),
            Description = "Verisoft API description",
            Contact = new OpenApiContact { Name = "Verisoft s.r.o.", Email = "info@verisoft.cz", Url = new Uri(VerisoftUrl) },
        };

        if (description.IsDeprecated)
        {
            info.Description += " This API version has been deprecated. Please use newer version of API.";
        }

        return info;
    }
}
