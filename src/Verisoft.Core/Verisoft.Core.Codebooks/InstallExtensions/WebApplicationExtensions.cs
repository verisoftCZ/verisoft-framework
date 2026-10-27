using System.Reflection;
using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Verisoft.Core.AspNet.Mvc;
using Verisoft.Core.Codebooks.Common.Filters;
using Verisoft.Core.Codebooks.Common.Models;
using Verisoft.Core.Codebooks.Configurations;
using Verisoft.Core.Codebooks.Constants;
using Verisoft.Core.Codebooks.Extensions;
using Verisoft.Core.Codebooks.Services;
using Verisoft.Core.Common.Entities.Codebooks;
using Verisoft.Core.Contracts;
using Verisoft.Core.Contracts.BusinessResult;

namespace Verisoft.Core.Codebooks.InstallExtensions;

public static class WebApplicationExtensions
{
    public static async Task<WebApplication> UseCodebookApi(this WebApplication webApplication)
    {
        await CodebookConfiguration.EnsureSeedCodebookTableDataAsync(webApplication.Services);
        webApplication.ConfigureCodebookRecordControllers();

        return webApplication;
    }

    private static WebApplication ConfigureCodebookRecordControllers(this WebApplication webApplication)
    {
        var configuration = webApplication.Services.GetService<MainCodebookRecordConfiguration>() ?? throw new InvalidOperationException("MainCodebookRecordConfiguration is not registered in the service collection.");
        var authorizationConfiguration = webApplication.Services.GetRequiredService<IOptions<CodebookAuthorizationConfiguration>>().Value;

        foreach (var codebookRecordConfig in configuration.CodebookRecordConfiguration)
        {
            var methodInfo = typeof(WebApplicationExtensions).GetMethod(nameof(ConfigureEndpointsV1), BindingFlags.Static | BindingFlags.NonPublic) ?? throw new InvalidOperationException("Method ConfigureEndpoints is not found.");

            var genericMethod = methodInfo.MakeGenericMethod(codebookRecordConfig.CodebookRecordType);
            genericMethod.Invoke(null, [webApplication, codebookRecordConfig, authorizationConfiguration, configuration.CodebookRecordRoutePrefix]);
        }

        return webApplication;
    }

    private static void ConfigureEndpointsV1<TCodebookRecordType>(
        WebApplication webApplication,
        CodebookRecordConfiguration codebookRecordConfiguration,
        CodebookAuthorizationConfiguration authorizationConfiguration,
        string prefix)
        where TCodebookRecordType : CodebookRecordType, new()
    {
        var apiVersionSet = webApplication.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1.0))
            .ReportApiVersions()
            .Build();

        var groupBuilder = webApplication
            .MapGroup("v{v:apiVersion}")
            .WithApiVersionSet(apiVersionSet)
            .RequireAuthorization();

        var codebookRecordServiceGenericType = typeof(ICodebookRecordService<TCodebookRecordType>);
        var stringValueServiceGenericType = typeof(IStringValueService<TCodebookRecordType>);

        GetDetail<TCodebookRecordType>(webApplication, codebookRecordConfiguration, prefix, groupBuilder, stringValueServiceGenericType);
        GetDetailList<TCodebookRecordType>(webApplication, codebookRecordConfiguration, prefix, groupBuilder, stringValueServiceGenericType);
        GetCodebookRecord<TCodebookRecordType>(webApplication, codebookRecordConfiguration, prefix, groupBuilder, codebookRecordServiceGenericType);
        GetCodebookRecordList<TCodebookRecordType>(webApplication, codebookRecordConfiguration, prefix, groupBuilder, codebookRecordServiceGenericType);
        Add<TCodebookRecordType>(webApplication, codebookRecordConfiguration, authorizationConfiguration, prefix, groupBuilder, stringValueServiceGenericType);
        Delete<TCodebookRecordType>(webApplication, codebookRecordConfiguration, authorizationConfiguration, prefix, groupBuilder, stringValueServiceGenericType);
        Update<TCodebookRecordType>(webApplication, codebookRecordConfiguration, authorizationConfiguration, prefix, groupBuilder, stringValueServiceGenericType);
        AddTranslation<TCodebookRecordType>(webApplication, codebookRecordConfiguration, authorizationConfiguration, prefix, groupBuilder, stringValueServiceGenericType);
        RemoveTranslation<TCodebookRecordType>(webApplication, codebookRecordConfiguration, authorizationConfiguration, prefix, groupBuilder, stringValueServiceGenericType);
        IsAvailable<TCodebookRecordType>(webApplication, codebookRecordConfiguration, prefix, groupBuilder, codebookRecordServiceGenericType);
        CustomEndpoints(codebookRecordConfiguration, prefix, groupBuilder);
    }

    private static void CustomEndpoints(CodebookRecordConfiguration codebookRecordConfiguration, string prefix, RouteGroupBuilder groupBuilder)
    {
        foreach (var customEndpoint in codebookRecordConfiguration.CustomEndpoints)
        {
            var fullRoute = GetRoute(prefix, codebookRecordConfiguration.CodebookRecordEndpointName, customEndpoint.Route);
            var endpointBuilder = customEndpoint.Method switch
            {
                HttpMethod m when m == HttpMethod.Get => groupBuilder.MapGet(fullRoute, customEndpoint.Handler),
                HttpMethod m when m == HttpMethod.Post => groupBuilder.MapPost(fullRoute, customEndpoint.Handler),
                HttpMethod m when m == HttpMethod.Put => groupBuilder.MapPut(fullRoute, customEndpoint.Handler),
                HttpMethod m when m == HttpMethod.Delete => groupBuilder.MapDelete(fullRoute, customEndpoint.Handler),
                HttpMethod m when m == HttpMethod.Patch => groupBuilder.MapPatch(fullRoute, customEndpoint.Handler),
                _ => throw new NotSupportedException($"HTTP method {customEndpoint.Method} is not supported"),
            };

            endpointBuilder.WithTags(codebookRecordConfiguration.CodebookRecordEndpointName);

            if (!string.IsNullOrWhiteSpace(customEndpoint.Name))
            {
                endpointBuilder.WithName(customEndpoint.Name);
            }

            if (!string.IsNullOrWhiteSpace(customEndpoint.Description))
            {
                endpointBuilder.WithDescription(customEndpoint.Description);
            }

            foreach (var (responseType, statusCode) in customEndpoint.ResponseTypes)
            {
                endpointBuilder.Produces(statusCode, responseType);
            }

            var permissionConfiguration = codebookRecordConfiguration.CodebookRecordRequiredPermissionConfiguration;
            _ = permissionConfiguration.CustomPermissions.TryGetValue(customEndpoint.Name ?? customEndpoint.Route, out var customPermissions);
            if (customPermissions?.Count is not null and not 0)
            {
                endpointBuilder.RequirePermission([.. customPermissions]);
            }
        }
    }

    private static void IsAvailable<TCodebookRecordType>(WebApplication webApplication, CodebookRecordConfiguration codebookRecordConfiguration, string prefix, RouteGroupBuilder groupBuilder, Type codebookRecordServiceGenericType)
        where TCodebookRecordType : CodebookRecordType, new()
    {
        var permissionConfiguration = codebookRecordConfiguration.CodebookRecordRequiredPermissionConfiguration;
        _ = permissionConfiguration.CustomPermissions.TryGetValue(MethodBase.GetCurrentMethod()?.Name ?? string.Empty, out var customPermissions);
        var requiredPermissions = new List<string>((customPermissions ?? []).Union(permissionConfiguration.GlobalReadPermissions));

        var endpointBuilder = groupBuilder
                    .MapGet(
                        GetRoute(prefix, codebookRecordConfiguration.CodebookRecordEndpointName, "{stringValue}/IsAvailable"),
                        async (string stringValue) =>
                        {
                            var scope = webApplication.Services.CreateScope();
                            var service = scope.ServiceProvider.GetService(codebookRecordServiceGenericType) as ICodebookRecordService<TCodebookRecordType> ?? throw new InvalidOperationException($"Service {codebookRecordServiceGenericType.Name} is not registered in the service collection.");
                            var isAvailable = await service.IsCodebookRecordAvailableAsync(stringValue);
                            return ConvertIActionResultToIResult(isAvailable.ToActionResult());
                        })
                    .WithTags(codebookRecordConfiguration.CodebookRecordEndpointName)
                    .WithName($"{codebookRecordConfiguration.CodebookRecordEndpointName}_IsAvailable")
                    .Produces(StatusCodes.Status200OK, typeof(bool))
                    .Produces(StatusCodes.Status400BadRequest, typeof(BadRequestResponse))
                    .Produces(StatusCodes.Status401Unauthorized, typeof(UnauthorizedResult));

        if (requiredPermissions.Count > 0)
        {
            endpointBuilder.RequirePermission([.. requiredPermissions]);
        }
    }

    private static void RemoveTranslation<TCodebookRecordType>(
        WebApplication webApplication,
        CodebookRecordConfiguration codebookRecordConfiguration,
        CodebookAuthorizationConfiguration codebookAuthorizationConfiguration,
        string prefix,
        RouteGroupBuilder groupBuilder,
        Type stringValueServiceGenericType)
        where TCodebookRecordType : CodebookRecordType, new()
    {
        var permissionConfiguration = codebookRecordConfiguration.CodebookRecordRequiredPermissionConfiguration;
        _ = permissionConfiguration.CustomPermissions.TryGetValue(MethodBase.GetCurrentMethod()?.Name ?? string.Empty, out var customPermissions);
        var requiredPermissions = new List<string>((customPermissions ?? [])
            .Union(permissionConfiguration.GlobalEditPermissions)
            .Union(codebookAuthorizationConfiguration.CanEditCodebook));

        var endpointBuilder = groupBuilder
                    .MapDelete(
                    GetRoute(prefix, codebookRecordConfiguration.CodebookRecordEndpointName, "{stringValue}/Translation/{languageCode}"),
                    async (string stringValue, string languageCode) =>
                    {
                        var scope = webApplication.Services.CreateScope();
                        var service = scope.ServiceProvider.GetService(stringValueServiceGenericType) as IStringValueService<TCodebookRecordType> ?? throw new InvalidOperationException($"Service {stringValueServiceGenericType.Name} is not registered in the service collection.");
                        var result = await service.RemoveTranslationAsync(stringValue, languageCode);
                        return ConvertIActionResultToIResult(result.ToActionResult());
                    })
                    .WithTags(codebookRecordConfiguration.CodebookRecordEndpointName)
                    .WithName($"{codebookRecordConfiguration.CodebookRecordEndpointName}_RemoveTranslation")
                    .Produces(StatusCodes.Status200OK, typeof(StringValueTranslations))
                    .Produces(StatusCodes.Status400BadRequest, typeof(BadRequestResponse))
                    .Produces(StatusCodes.Status401Unauthorized, typeof(UnauthorizedResult))
                    .Produces(StatusCodes.Status403Forbidden, typeof(BusinessActionErrors))
                    .Produces(StatusCodes.Status404NotFound, typeof(BusinessActionErrors));

        if (requiredPermissions.Count > 0)
        {
            endpointBuilder.RequirePermission([.. requiredPermissions]);
        }
    }

    private static void AddTranslation<TCodebookRecordType>(
        WebApplication webApplication,
        CodebookRecordConfiguration codebookRecordConfiguration,
        CodebookAuthorizationConfiguration codebookAuthorizationConfiguration,
        string prefix,
        RouteGroupBuilder groupBuilder,
        Type stringValueServiceGenericType)
        where TCodebookRecordType : CodebookRecordType, new()
    {
        var permissionConfiguration = codebookRecordConfiguration.CodebookRecordRequiredPermissionConfiguration;
        _ = permissionConfiguration.CustomPermissions.TryGetValue(MethodBase.GetCurrentMethod()?.Name ?? string.Empty, out var customPermissions);
        var requiredPermissions = new List<string>((customPermissions ?? [])
            .Union(permissionConfiguration.GlobalEditPermissions)
            .Union(codebookAuthorizationConfiguration.CanEditCodebook));

        var endpointBuilder = groupBuilder
                    .MapPost(
                    GetRoute(prefix, codebookRecordConfiguration.CodebookRecordEndpointName, "{stringValue}/Translation/{languageCode}/{value}"),
                    async (string stringValue, string languageCode, string value) =>
                    {
                        var scope = webApplication.Services.CreateScope();
                        var service = scope.ServiceProvider.GetService(stringValueServiceGenericType) as IStringValueService<TCodebookRecordType> ?? throw new InvalidOperationException($"Service {stringValueServiceGenericType.Name} is not registered in the service collection.");
                        var result = await service.UpsertTranslationAsync(stringValue, languageCode, value);
                        return ConvertIActionResultToIResult(result.ToActionResult());
                    })
                    .WithTags(codebookRecordConfiguration.CodebookRecordEndpointName)
                    .WithName($"{codebookRecordConfiguration.CodebookRecordEndpointName}_AddTranslation")
                    .Produces(StatusCodes.Status200OK, typeof(StringValueTranslations))
                    .Produces(StatusCodes.Status401Unauthorized, typeof(UnauthorizedResult))
                    .Produces(StatusCodes.Status403Forbidden, typeof(BusinessActionErrors))
                    .Produces(StatusCodes.Status404NotFound, typeof(BusinessActionErrors));

        if (requiredPermissions.Count > 0)
        {
            endpointBuilder.RequirePermission([.. requiredPermissions]);
        }
    }

    private static void Update<TCodebookRecordType>(
        WebApplication webApplication,
        CodebookRecordConfiguration codebookRecordConfiguration,
        CodebookAuthorizationConfiguration codebookAuthorizationConfiguration,
        string prefix,
        RouteGroupBuilder groupBuilder,
        Type stringValueServiceGenericType)
        where TCodebookRecordType : CodebookRecordType, new()
    {
        var permissionConfiguration = codebookRecordConfiguration.CodebookRecordRequiredPermissionConfiguration;
        _ = permissionConfiguration.CustomPermissions.TryGetValue(MethodBase.GetCurrentMethod()?.Name ?? string.Empty, out var customPermissions);
        var requiredPermissions = new List<string>((customPermissions ?? [])
            .Union(permissionConfiguration.GlobalEditPermissions)
            .Union(codebookAuthorizationConfiguration.CanEditCodebook));

        var endpointBuilder = groupBuilder
                    .MapPut(
                        GetRoute(prefix, codebookRecordConfiguration.CodebookRecordEndpointName, "{stringValue}"),
                        async (string stringValue, [FromBody] StringValueDetailEditModel<TCodebookRecordType> codebookRecordDefinitionEditModel) =>
                        {
                            var scope = webApplication.Services.CreateScope();
                            var service = scope.ServiceProvider.GetService(stringValueServiceGenericType) as IStringValueService<TCodebookRecordType> ?? throw new InvalidOperationException($"Service {stringValueServiceGenericType.Name} is not registered in the service collection.");
                            var validatedModel = await service.ValidateAsync(stringValue, codebookRecordDefinitionEditModel, false);
                            if (!validatedModel.IsSuccess)
                            {
                                return Results.UnprocessableEntity(validatedModel);
                            }

                            var codebookRecordDefinition = await service.UpdateStringValueAsync(stringValue, codebookRecordDefinitionEditModel);
                            return ConvertIActionResultToIResult(codebookRecordDefinition.ToActionResult());
                        })
                    .WithTags(codebookRecordConfiguration.CodebookRecordEndpointName)
                    .Produces(StatusCodes.Status200OK, typeof(StringValueDetail<TCodebookRecordType>))
                    .Produces(StatusCodes.Status400BadRequest, typeof(BadRequestResponse))
                    .Produces(StatusCodes.Status401Unauthorized, typeof(UnauthorizedResult))
                    .Produces(StatusCodes.Status403Forbidden, typeof(BusinessActionErrors))
                    .Produces(StatusCodes.Status404NotFound, typeof(BusinessActionErrors))
                    .Produces(StatusCodes.Status422UnprocessableEntity, typeof(BusinessActionErrors<StringValueDetail<TCodebookRecordType>>));

        if (requiredPermissions.Count > 0)
        {
            endpointBuilder.RequirePermission([.. requiredPermissions]);
        }
    }

    private static void Delete<TCodebookRecordType>(
        WebApplication webApplication,
        CodebookRecordConfiguration codebookRecordConfiguration,
        CodebookAuthorizationConfiguration codebookAuthorizationConfiguration,
        string prefix,
        RouteGroupBuilder groupBuilder,
        Type stringValueServiceGenericType)
        where TCodebookRecordType : CodebookRecordType, new()
    {
        var permissionConfiguration = codebookRecordConfiguration.CodebookRecordRequiredPermissionConfiguration;
        _ = permissionConfiguration.CustomPermissions.TryGetValue(MethodBase.GetCurrentMethod()?.Name ?? string.Empty, out var customPermissions);
        var requiredPermissions = new List<string>((customPermissions ?? [])
            .Union(permissionConfiguration.GlobalEditPermissions)
            .Union(codebookAuthorizationConfiguration.CanEditCodebook));

        var endpointBuilder = groupBuilder
                    .MapDelete(
                        GetRoute(prefix, codebookRecordConfiguration.CodebookRecordEndpointName, "{stringValue}"),
                        async (string stringValue) =>
                        {
                            var scope = webApplication.Services.CreateScope();
                            var service = scope.ServiceProvider.GetService(stringValueServiceGenericType) as IStringValueService<TCodebookRecordType> ?? throw new InvalidOperationException($"Service {stringValueServiceGenericType.Name} is not registered in the service collection.");
                            var codebookRecordDefinition = await service.RemoveStringValueAsync(stringValue);
                            return ConvertIActionResultToIResult(codebookRecordDefinition.ToActionResult());
                        })
                    .WithTags(codebookRecordConfiguration.CodebookRecordEndpointName)
                    .Produces(StatusCodes.Status200OK)
                    .Produces(StatusCodes.Status400BadRequest, typeof(BadRequestResponse))
                    .Produces(StatusCodes.Status401Unauthorized, typeof(UnauthorizedResult))
                    .Produces(StatusCodes.Status403Forbidden, typeof(BusinessActionErrors))
                    .Produces(StatusCodes.Status404NotFound, typeof(BusinessActionErrors));

        if (requiredPermissions.Count > 0)
        {
            endpointBuilder.RequirePermission([.. requiredPermissions]);
        }
    }

    private static void Add<TCodebookRecordType>(
        WebApplication webApplication,
        CodebookRecordConfiguration codebookRecordConfiguration,
        CodebookAuthorizationConfiguration codebookAuthorizationConfiguration,
        string prefix,
        RouteGroupBuilder groupBuilder,
        Type stringValueServiceGenericType)
        where TCodebookRecordType : CodebookRecordType, new()
    {
        var permissionConfiguration = codebookRecordConfiguration.CodebookRecordRequiredPermissionConfiguration;
        _ = permissionConfiguration.CustomPermissions.TryGetValue(MethodBase.GetCurrentMethod()?.Name ?? string.Empty, out var customPermissions);
        var requiredPermissions = new List<string>((customPermissions ?? [])
            .Union(permissionConfiguration.GlobalEditPermissions)
            .Union(codebookAuthorizationConfiguration.CanEditCodebook));

        var endpointBuilder = groupBuilder
                    .MapPost(
                        GetRoute(prefix, codebookRecordConfiguration.CodebookRecordEndpointName),
                        async ([FromBody] StringValueDetailCreateModel<TCodebookRecordType> stringValueDetail) =>
                        {
                            var scope = webApplication.Services.CreateScope();
                            var service = scope.ServiceProvider.GetService(stringValueServiceGenericType) as IStringValueService<TCodebookRecordType> ?? throw new InvalidOperationException($"Service {stringValueServiceGenericType.Name} is not registered in the service collection.");
                            var validatedModel = await service.ValidateAsync(stringValueDetail.StringValue, stringValueDetail, true);
                            if (!validatedModel.IsSuccess)
                            {
                                return Results.UnprocessableEntity(validatedModel);
                            }

                            var result = await service.AddStringValueAsync(stringValueDetail);

                            return ConvertIActionResultToIResult(result.ToActionResult());
                        })
                    .WithTags(codebookRecordConfiguration.CodebookRecordEndpointName)
                    .Produces(StatusCodes.Status200OK, typeof(StringValueDetail<TCodebookRecordType>))
                    .Produces(StatusCodes.Status400BadRequest, typeof(BusinessActionErrors))
                    .Produces(StatusCodes.Status401Unauthorized, typeof(BusinessActionErrors))
                    .Produces(StatusCodes.Status403Forbidden, typeof(BusinessActionErrors))
                    .Produces(StatusCodes.Status422UnprocessableEntity, typeof(BusinessActionErrors<StringValueDetailValidatedModel<TCodebookRecordType>>));

        if (requiredPermissions.Count > 0)
        {
            endpointBuilder.RequirePermission([.. requiredPermissions]);
        }
    }

    private static void GetDetailList<TCodebookRecordType>(WebApplication webApplication, CodebookRecordConfiguration codebookRecordConfiguration, string prefix, RouteGroupBuilder groupBuilder, Type stringValueServiceGenericType)
        where TCodebookRecordType : CodebookRecordType, new()
    {
        var permissionConfiguration = codebookRecordConfiguration.CodebookRecordRequiredPermissionConfiguration;
        _ = permissionConfiguration.CustomPermissions.TryGetValue(MethodBase.GetCurrentMethod()?.Name ?? string.Empty, out var customPermissions);
        var requiredPermissions = new List<string>((customPermissions ?? []).Union(permissionConfiguration.GlobalReadPermissions));

        var endpointBuilder = groupBuilder
                    .MapGet(
                        GetRoute(prefix, codebookRecordConfiguration.CodebookRecordEndpointName, "DetailList"),
                        async (
                            [FromQuery] int? offset,
                            [FromQuery] int? limit,
                            [FromQuery(Name = "filter.searchTerm")] string? searchTerm,
                            [FromQuery(Name = "sort.property")] string? sortProperty,
                            [FromQuery(Name = "sort.direction")] SortDirection? direction) =>
                        {
                            var scope = webApplication.Services.CreateScope();
                            var service = scope.ServiceProvider.GetService(stringValueServiceGenericType) as IStringValueService<TCodebookRecordType> ?? throw new InvalidOperationException($"Service {stringValueServiceGenericType.Name} is not registered in the service collection.");
                            var filter = new StringValueDetailFilter
                            {
                                SearchTerm = searchTerm,
                            };

                            var request = new FilteredRequest<StringValueDetailFilter>
                            {
                                Offset = offset ?? 0,
                                Limit = limit ?? int.MaxValue,
                                Filter = filter,
                                Sorts = sortProperty != null ? new List<string> { $"{sortProperty},{direction ?? SortDirection.Asc}" } : null,
                            };

                            var result = await service.GetStringValueDetailListAsync(request);

                            return ConvertIActionResultToIResult(result.ToActionResult());
                        })
                    .WithTags(codebookRecordConfiguration.CodebookRecordEndpointName)
                    .WithName($"{codebookRecordConfiguration.CodebookRecordEndpointName}_DetailList")
                    .Produces(StatusCodes.Status200OK, typeof(StringValueDetailListData<TCodebookRecordType>))
                    .Produces(StatusCodes.Status400BadRequest, typeof(BadRequestResponse))
                    .Produces(StatusCodes.Status401Unauthorized, typeof(UnauthorizedResult));

        if (requiredPermissions.Count > 0)
        {
            endpointBuilder.RequirePermission([.. requiredPermissions]);
        }
    }

    private static void GetCodebookRecord<TCodebookRecordType>(WebApplication webApplication, CodebookRecordConfiguration codebookRecordConfiguration, string prefix, RouteGroupBuilder groupBuilder, Type codebookRecordServiceGenericType)
        where TCodebookRecordType : CodebookRecordType, new()
    {
        var permissionConfiguration = codebookRecordConfiguration.CodebookRecordRequiredPermissionConfiguration;
        _ = permissionConfiguration.CustomPermissions.TryGetValue(MethodBase.GetCurrentMethod()?.Name ?? string.Empty, out var customPermissions);
        var requiredPermissions = new List<string>((customPermissions ?? []).Union(permissionConfiguration.GlobalReadPermissions));

        var endpointBuilder = groupBuilder
                    .MapGet(
                        GetRoute(prefix, codebookRecordConfiguration.CodebookRecordEndpointName, "{stringValue}"),
                        async (string stringValue, [FromQuery] string? languageCode) =>
                        {
                            var scope = webApplication.Services.CreateScope();
                            var service = scope.ServiceProvider.GetService(codebookRecordServiceGenericType) as ICodebookRecordService<TCodebookRecordType> ?? throw new InvalidOperationException($"Service {codebookRecordServiceGenericType.Name} is not registered in the service collection.");
                            var result = await service.GetCodebookRecordAsync(stringValue, languageCode);
                            return ConvertIActionResultToIResult(result.ToActionResult());
                        })
                    .WithTags(codebookRecordConfiguration.CodebookRecordEndpointName)
                    .WithName($"{codebookRecordConfiguration.CodebookRecordEndpointName}")
                    .Produces(StatusCodes.Status200OK, typeof(CodebookRecord<TCodebookRecordType>))
                    .Produces(StatusCodes.Status400BadRequest, typeof(BadRequestResponse))
                    .Produces(StatusCodes.Status401Unauthorized, typeof(UnauthorizedResult))
                    .Produces(StatusCodes.Status404NotFound, typeof(BusinessActionErrors));

        if (requiredPermissions.Count > 0)
        {
            endpointBuilder.RequirePermission([.. requiredPermissions]);
        }
    }

    private static void GetCodebookRecordList<TCodebookRecordType>(WebApplication webApplication, CodebookRecordConfiguration codebookRecordConfiguration, string prefix, RouteGroupBuilder groupBuilder, Type codebookRecordServiceGenericType)
        where TCodebookRecordType : CodebookRecordType, new()
    {
        var permissionConfiguration = codebookRecordConfiguration.CodebookRecordRequiredPermissionConfiguration;
        _ = permissionConfiguration.CustomPermissions.TryGetValue(MethodBase.GetCurrentMethod()?.Name ?? string.Empty, out var customPermissions);
        var requiredPermissions = new List<string>((customPermissions ?? []).Union(permissionConfiguration.GlobalReadPermissions));

        var endpointBuilder = groupBuilder
            .MapGet(
                GetRoute(prefix, codebookRecordConfiguration.CodebookRecordEndpointName),
                async (
                    [FromQuery] int? limit,
                    [FromQuery] int? offset,
                    [FromQuery(Name = "filter.translation")] string? translation,
                    [FromQuery(Name = "filter.languageCode")] string? languageCode,
                    [FromQuery(Name = "filter.exclude")] string[]? exclude,
                    [FromQuery(Name = "filter.stringValue")] string[]? stringValue,
                    [FromQuery(Name = "sort.property")] string? sortProperty,
                    [FromQuery(Name = "sort.direction")] SortDirection? direction) =>
                {
                    var scope = webApplication.Services.CreateScope();
                    var service = scope.ServiceProvider.GetService(codebookRecordServiceGenericType) as ICodebookRecordService<TCodebookRecordType> ?? throw new InvalidOperationException($"Service {codebookRecordServiceGenericType.Name} is not registered in the service collection.");
                    var filter = new CodebookRecordFilter
                    {
                        Translation = translation ?? string.Empty,
                        Exclude = exclude ?? [],
                        StringValue = stringValue ?? [],
                        LanguageCode = languageCode ?? LanguageConstants.DefaultLanguageCode,
                    };

                    var request = new FilteredRequest<CodebookRecordFilter>
                    {
                        Offset = offset ?? 0,
                        Limit = limit ?? int.MaxValue,
                        Filter = filter,
                        Sorts = sortProperty != null ? new List<string> { $"{sortProperty},{direction ?? SortDirection.Asc}" } : null,
                    };

                    var result = await service.GetCodebookRecordsAsync(request);

                    return ConvertIActionResultToIResult(result.ToActionResult());
                })
            .WithTags(codebookRecordConfiguration.CodebookRecordEndpointName)
            .Produces(StatusCodes.Status200OK, typeof(CodebookRecordListData<TCodebookRecordType>))
            .Produces(StatusCodes.Status400BadRequest, typeof(BadRequestResponse))
            .Produces(StatusCodes.Status401Unauthorized, typeof(UnauthorizedResult));

        if (requiredPermissions.Count > 0)
        {
            endpointBuilder.RequirePermission([.. requiredPermissions]);
        }
    }

    private static void GetDetail<TCodebookRecordType>(WebApplication webApplication, CodebookRecordConfiguration codebookRecordConfiguration, string prefix, RouteGroupBuilder groupBuilder, Type stringValueServiceGenericType)
        where TCodebookRecordType : CodebookRecordType, new()
    {
        var permissionConfiguration = codebookRecordConfiguration.CodebookRecordRequiredPermissionConfiguration;
        _ = permissionConfiguration.CustomPermissions.TryGetValue(MethodBase.GetCurrentMethod()?.Name ?? string.Empty, out var customPermissions);
        var requiredPermissions = new List<string>((customPermissions ?? []).Union(permissionConfiguration.GlobalReadPermissions));

        var endpointBuilder = groupBuilder
                    .MapGet(
                        GetRoute(prefix, codebookRecordConfiguration.CodebookRecordEndpointName, "{stringValue}/Detail"),
                        async (string stringValue) =>
                        {
                            var scope = webApplication.Services.CreateScope();
                            var service = scope.ServiceProvider.GetService(stringValueServiceGenericType) as IStringValueService<TCodebookRecordType> ?? throw new InvalidOperationException($"Service {stringValueServiceGenericType.Name} is not registered in the service collection.");
                            var result = await service.GetStringValueDetailAsync(stringValue);
                            return ConvertIActionResultToIResult(result.ToActionResult());
                        })
                    .WithTags(codebookRecordConfiguration.CodebookRecordEndpointName)
                    .WithName($"{codebookRecordConfiguration.CodebookRecordEndpointName}_Detail")
                    .Produces(StatusCodes.Status200OK, typeof(StringValueDetail<TCodebookRecordType>))
                    .Produces(StatusCodes.Status400BadRequest, typeof(BusinessActionErrors))
                    .Produces(StatusCodes.Status401Unauthorized, typeof(UnauthorizedResult))
                    .Produces(StatusCodes.Status404NotFound, typeof(BusinessActionErrors));

        if (requiredPermissions.Count > 0)
        {
            endpointBuilder.RequirePermission([.. requiredPermissions]);
        }
    }

    private static string GetRoute(params string[] parts)
    {
        var route = string.Join("/", parts.Where(p => !string.IsNullOrWhiteSpace(p)).Select(p => p.Trim().Trim('/')));
        return $"/{route}";
    }

    private static IResult ConvertIActionResultToIResult(IActionResult result)
    {
        return result switch
        {
            OkObjectResult okObjectResult => Results.Ok(okObjectResult.Value),
            OkResult => Results.Ok(),
            BadRequestObjectResult badRequestObjectResult => Results.BadRequest(badRequestObjectResult.Value),
            NotFoundObjectResult notFoundObjectResult => Results.NotFound(notFoundObjectResult.Value),
            ForbiddenObjectResult => Results.Forbid(),
            UnprocessableEntityObjectResult unprocessableEntityObjectResult => Results.UnprocessableEntity(unprocessableEntityObjectResult.Value),
            _ => Results.StatusCode(500),
        };
    }
}