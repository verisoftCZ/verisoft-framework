using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Verisoft.CodebookApi.Application.Services.Interfaces;
using Verisoft.CodebookApi.Contracts.Filters;
using Verisoft.CodebookApi.Contracts.Models.Base;
using Verisoft.CodebookApi.Contracts.Models.CodebookRecord;
using Verisoft.CodebookApi.Core.Configurations;
using Verisoft.CodebookApi.Core.Constants;
using Verisoft.Core.AspNet.Mvc;
using Verisoft.Core.Contracts;
using Verisoft.Core.Contracts.BusinessResult;
using Verisoft.Core.Contracts.Validation.Extensions;

namespace Verisoft.CodebookApi.Host.InstallExtensions;

public static class WebApplicationExtensions
{
    public static WebApplication ConfigureCodebookRecordControllers(this WebApplication webApplication)
    {
        var configuration = webApplication.Services.GetService<IMainCodebookRecordConfiguration>() ?? throw new InvalidOperationException("MainCodebookRecordConfiguration is not registered in the service collection.");
        foreach (var codebookRecordConfig in configuration.CodebookRecordConfiguration)
        {
            var entityType = codebookRecordConfig.CodebookRecordType;
            var propContractType = codebookRecordConfig.PropContract;
            var tenantContractType = codebookRecordConfig.TenantContract;

            var methodInfo = typeof(WebApplicationExtensions).GetMethod(nameof(ConfigureEndpointsV1), System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic) ?? throw new InvalidOperationException("Method ConfigureEndpoints is not found.");
            var genericMethod = methodInfo.MakeGenericMethod(propContractType, tenantContractType);
            genericMethod.Invoke(null, [webApplication, codebookRecordConfig, configuration.CodebookRecordRoutePrefix]);
        }

        return webApplication;
    }

    private static void ConfigureEndpointsV1<TAdditionalProperties, TTenantValue>(WebApplication webApplication, CodebookRecordConfiguration codebookRecordConfiguration, string prefix)
        where TAdditionalProperties : class, new()
        where TTenantValue : class, ITenantValue, new()
    {
        var apiVersionSet = webApplication.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1.0))
            .ReportApiVersions()
            .Build();

        var groupBuilder = webApplication
            .MapGroup("v{v:apiVersion}")
            .WithApiVersionSet(apiVersionSet)
            .RequireAuthorization();

        var codebookRecordServiceType = typeof(ICodebookRecordService<>);
        var codebookRecordServiceGenericType = codebookRecordServiceType.MakeGenericType(codebookRecordConfiguration.CodebookRecordType);
        var stringValueServiceType = typeof(IStringValueService<>);
        var stringValueServiceGenericType = stringValueServiceType.MakeGenericType(codebookRecordConfiguration.CodebookRecordType);

        groupBuilder
            .MapGet(
                GetRoute(prefix, codebookRecordConfiguration.CodebookRecordEndpointName, "{stringValue}/Detail"),
                async (string stringValue) =>
                    {
                        var scope = webApplication.Services.CreateScope();
                        var service = scope.ServiceProvider.GetService(stringValueServiceGenericType) as IStringValueService ?? throw new InvalidOperationException($"Service {stringValueServiceGenericType.Name} is not registered in the service collection.");
                        var result = await service.GetStringValueDetailAsync<TAdditionalProperties, TTenantValue>(stringValue);
                        return ConvertIActionResultToIResult(result.ToActionResult());
                    })
            .WithTags(codebookRecordConfiguration.CodebookRecordEndpointName)
            .WithName($"{codebookRecordConfiguration.CodebookRecordEndpointName}_Detail")
            .Produces(StatusCodes.Status200OK, typeof(StringValueDetail<TAdditionalProperties, TTenantValue>))
            .Produces(StatusCodes.Status400BadRequest, typeof(BusinessActionErrors))
            .Produces(StatusCodes.Status401Unauthorized, typeof(UnauthorizedResult))
            .Produces(StatusCodes.Status404NotFound, typeof(BusinessActionErrors))
            .Produces(StatusCodes.Status500InternalServerError);

        groupBuilder
            .MapGet(
                GetRoute(prefix, codebookRecordConfiguration.CodebookRecordEndpointName),
                async (
                    [FromQuery] int? limit,
                    [FromQuery] int? offset,
                    [FromQuery(Name = "filter.translation")] string? translation,
                    [FromQuery(Name = "filter.languageCode")] string? languageCode,
                    [FromQuery(Name = "filter.stringValueArrayToExclude")] string[]? stringValueArrayToExclude,
                    [FromQuery(Name = "filter.stringValueArray")] string[]? stringValueArray,
                    [FromQuery(Name = "sort.property")] string? sortProperty,
                    [FromQuery(Name = "sort.direction")] SortDirection? direction) =>
                        {
                            var scope = webApplication.Services.CreateScope();
                            var service = scope.ServiceProvider.GetService(codebookRecordServiceGenericType) as ICodebookRecordService ?? throw new InvalidOperationException($"Service {codebookRecordServiceGenericType.Name} is not registered in the service collection.");
                            var filter = new CodebookRecordFilter
                            {
                                Translation = translation ?? string.Empty,
                                StringValueArrayToExclude = stringValueArrayToExclude ?? [],
                                StringValueArray = stringValueArray ?? [],
                                LanguageCode = languageCode ?? LanguageConstants.DefaultLanguageCode,
                            };

                            var request = new FilteredRequest<CodebookRecordFilter>
                            {
                                Offset = offset ?? 0,
                                Limit = limit ?? int.MaxValue,
                                Filter = filter,
                                Sort = new SortDefinition
                                {
                                    Field = sortProperty ?? string.Empty,
                                    Direction = direction ?? SortDirection.Asc,
                                },
                            };

                            var result = await service.GetCodebookRecordsAsync<TAdditionalProperties, TTenantValue>(request);

                            return ConvertIActionResultToIResult(result.ToActionResult());
                        })
            .WithTags(codebookRecordConfiguration.CodebookRecordEndpointName)
            .Produces(StatusCodes.Status200OK, typeof(CodebookRecordListData<TAdditionalProperties>))
            .Produces(StatusCodes.Status400BadRequest, typeof(BadRequestResponse))
            .Produces(StatusCodes.Status401Unauthorized, typeof(UnauthorizedResult))
            .Produces(StatusCodes.Status500InternalServerError);

        groupBuilder
            .MapGet(
                GetRoute(prefix, codebookRecordConfiguration.CodebookRecordEndpointName, "{stringValue}"),
                async (string stringValue, [FromQuery] string? languageCode) =>
                {
                    var scope = webApplication.Services.CreateScope();
                    var service = scope.ServiceProvider.GetService(codebookRecordServiceGenericType) as ICodebookRecordService ?? throw new InvalidOperationException($"Service {codebookRecordServiceGenericType.Name} is not registered in the service collection.");
                    var result = await service.GetCodebookRecordAsync<TAdditionalProperties, TTenantValue>(stringValue, languageCode);
                    return ConvertIActionResultToIResult(result.ToActionResult());
                })
            .WithTags(codebookRecordConfiguration.CodebookRecordEndpointName)
            .WithName($"{codebookRecordConfiguration.CodebookRecordEndpointName}")
            .Produces(StatusCodes.Status200OK, typeof(CodebookRecord<TAdditionalProperties>))
            .Produces(StatusCodes.Status400BadRequest, typeof(BadRequestResponse))
            .Produces(StatusCodes.Status401Unauthorized, typeof(UnauthorizedResult))
            .Produces(StatusCodes.Status404NotFound, typeof(BusinessActionErrors))
            .Produces(StatusCodes.Status500InternalServerError);

        groupBuilder
            .MapGet(
                GetRoute(prefix, codebookRecordConfiguration.CodebookRecordEndpointName, "DetailList"),
                async (
                    [FromQuery] int? offset,
                    [FromQuery] int? limit,
                    [FromQuery(Name = "filter.searchTerm")] string? searchTerm,
                    [FromQuery(Name = "filter.hasExceptionalValues")] bool? hasExceptionalValues,
                    [FromQuery(Name = "filter.tenantId")] int? tenantId,
                    [FromQuery(Name = "sort.property")] string? sortProperty,
                    [FromQuery(Name = "sort.direction")] SortDirection? direction) =>
                        {
                            var scope = webApplication.Services.CreateScope();
                            var service = scope.ServiceProvider.GetService(stringValueServiceGenericType) as IStringValueService ?? throw new InvalidOperationException($"Service {stringValueServiceGenericType.Name} is not registered in the service collection.");
                            var filter = new StringValueDetailFilter
                            {
                                SearchTerm = searchTerm,
                                HasExceptionalValues = hasExceptionalValues,
                                TenantId = tenantId,
                            };

                            var request = new FilteredRequest<StringValueDetailFilter>
                            {
                                Offset = offset ?? 0,
                                Limit = limit ?? int.MaxValue,
                                Filter = filter,
                                Sort = new SortDefinition
                                {
                                    Field = sortProperty ?? string.Empty,
                                    Direction = direction ?? SortDirection.Asc,
                                },
                            };

                            var result = await service.GetStringValueDetailListAsync<TAdditionalProperties, TTenantValue>(request);

                            return ConvertIActionResultToIResult(result.ToActionResult());
                        })
            .WithTags(codebookRecordConfiguration.CodebookRecordEndpointName)
            .WithName($"{codebookRecordConfiguration.CodebookRecordEndpointName}_DetailList")
            .Produces(StatusCodes.Status200OK, typeof(StringValueDetailListData<TAdditionalProperties, TTenantValue>))
            .Produces(StatusCodes.Status400BadRequest, typeof(BadRequestResponse))
            .Produces(StatusCodes.Status401Unauthorized, typeof(UnauthorizedResult))
            .Produces(StatusCodes.Status500InternalServerError);

        groupBuilder
            .MapPost(
                GetRoute(prefix, codebookRecordConfiguration.CodebookRecordEndpointName),
                async ([FromBody] StringValueDetail<TAdditionalProperties, TTenantValue> stringValueDetail) =>
                    {
                        var scope = webApplication.Services.CreateScope();
                        var service = scope.ServiceProvider.GetService(stringValueServiceGenericType) as IStringValueService ?? throw new InvalidOperationException($"Service {stringValueServiceGenericType.Name} is not registered in the service collection.");
                        var validatedModel = await service.ValidateAsync(stringValueDetail.StringValue, stringValueDetail.IsGlobal, stringValueDetail, true);
                        if (!validatedModel.IsSuccess)
                        {
                            return Results.UnprocessableEntity(validatedModel);
                        }

                        var result = await service.AddStringValueAsync(stringValueDetail);

                        return ConvertIActionResultToIResult(result.ToActionResult());
                    })
            .WithTags(codebookRecordConfiguration.CodebookRecordEndpointName)
            .Produces(StatusCodes.Status200OK, typeof(StringValueDetail<TAdditionalProperties, TTenantValue>))
            .Produces(StatusCodes.Status400BadRequest, typeof(BusinessActionErrors))
            .Produces(StatusCodes.Status401Unauthorized, typeof(BusinessActionErrors))
            .Produces(StatusCodes.Status422UnprocessableEntity, typeof(BusinessActionErrors<StringValueDetailValidatedModel<TAdditionalProperties, TTenantValue>>))
            .Produces(StatusCodes.Status500InternalServerError);

        groupBuilder
            .MapPost(
                GetRoute(prefix, codebookRecordConfiguration.CodebookRecordEndpointName, "AddBulk"),
                async ([FromBody] StringValueBulkAddModel<TAdditionalProperties, TTenantValue> stringValueBulkAddModel) =>
                    {
                        var scope = webApplication.Services.CreateScope();
                        var service = scope.ServiceProvider.GetService(stringValueServiceGenericType) as IStringValueService ?? throw new InvalidOperationException($"Service {stringValueServiceGenericType.Name} is not registered in the service collection.");
                        var result = await service.AddStringValuesInBulkAsync(stringValueBulkAddModel, codebookRecordConfiguration.CodebookRecordTableName);

                        return ConvertIActionResultToIResult(result.ToActionResult());
                    })
            .WithTags(codebookRecordConfiguration.CodebookRecordEndpointName)
            .WithName($"{codebookRecordConfiguration.CodebookRecordEndpointName}_AddBulk")
            .Produces(StatusCodes.Status200OK, typeof(StringValueDetail<TAdditionalProperties, TTenantValue>))
            .Produces(StatusCodes.Status400BadRequest, typeof(BadRequestResponse))
            .Produces(StatusCodes.Status401Unauthorized, typeof(UnauthorizedResult))
            .Produces(StatusCodes.Status422UnprocessableEntity, typeof(BusinessActionErrors))
            .Produces(StatusCodes.Status500InternalServerError);

        groupBuilder
            .MapDelete(
                GetRoute(prefix, codebookRecordConfiguration.CodebookRecordEndpointName, "{stringValue}"),
                async (string stringValue) =>
                    {
                        var scope = webApplication.Services.CreateScope();
                        var service = scope.ServiceProvider.GetService(stringValueServiceGenericType) as IStringValueService ?? throw new InvalidOperationException($"Service {stringValueServiceGenericType.Name} is not registered in the service collection.");
                        var codebookRecordDefinition = await service.RemoveStringValueAsync<TAdditionalProperties, TTenantValue>(stringValue);
                        return ConvertIActionResultToIResult(codebookRecordDefinition.ToActionResult());
                    })
            .WithTags(codebookRecordConfiguration.CodebookRecordEndpointName)
            .Produces(StatusCodes.Status200OK, typeof(StringValueDetail<TAdditionalProperties, TTenantValue>))
            .Produces(StatusCodes.Status400BadRequest, typeof(BadRequestResponse))
            .Produces(StatusCodes.Status401Unauthorized, typeof(UnauthorizedResult))
            .Produces(StatusCodes.Status404NotFound, typeof(BusinessActionErrors))
            .Produces(StatusCodes.Status500InternalServerError);

        groupBuilder
            .MapPut(
                GetRoute(prefix, codebookRecordConfiguration.CodebookRecordEndpointName, "{stringValue}"),
                async (string stringValue, [FromBody] StringValueDetailEditModel<TAdditionalProperties, TTenantValue> codebookRecordDefinitionEditModel) =>
                    {
                        var scope = webApplication.Services.CreateScope();
                        var service = scope.ServiceProvider.GetService(stringValueServiceGenericType) as IStringValueService ?? throw new InvalidOperationException($"Service {stringValueServiceGenericType.Name} is not registered in the service collection.");
                        var validatedModel = await service.ValidateAsync(stringValue, null, codebookRecordDefinitionEditModel, false);
                        if (!validatedModel.IsSuccess)
                        {
                            return Results.UnprocessableEntity(validatedModel);
                        }

                        var codebookRecordDefinition = await service.UpdateStringValueAsync(stringValue, codebookRecordDefinitionEditModel);
                        return ConvertIActionResultToIResult(codebookRecordDefinition.ToActionResult());
                    })
            .WithTags(codebookRecordConfiguration.CodebookRecordEndpointName)
            .Produces(StatusCodes.Status200OK, typeof(StringValueDetail<TAdditionalProperties, TTenantValue>))
            .Produces(StatusCodes.Status400BadRequest, typeof(BadRequestResponse))
            .Produces(StatusCodes.Status401Unauthorized, typeof(UnauthorizedResult))
            .Produces(StatusCodes.Status404NotFound, typeof(BusinessActionErrors))
            .Produces(StatusCodes.Status422UnprocessableEntity, typeof(BusinessActionErrors<StringValueDetail<TAdditionalProperties, TTenantValue>>))
            .Produces(StatusCodes.Status500InternalServerError);

        groupBuilder
            .MapGet(
                GetRoute(prefix, codebookRecordConfiguration.CodebookRecordEndpointName, "{stringValue}/IsAvailable"),
                async (string stringValue) =>
                    {
                        var scope = webApplication.Services.CreateScope();
                        var service = scope.ServiceProvider.GetService(codebookRecordServiceGenericType) as ICodebookRecordService ?? throw new InvalidOperationException($"Service {codebookRecordServiceGenericType.Name} is not registered in the service collection.");
                        var isAvailable = await service.IsCodebookRecordAvailableAsync<TAdditionalProperties, TTenantValue>(stringValue);
                        return ConvertIActionResultToIResult(isAvailable.ToActionResult());
                    })
            .WithTags(codebookRecordConfiguration.CodebookRecordEndpointName)
            .WithName($"{codebookRecordConfiguration.CodebookRecordEndpointName}_IsAvailable")
            .Produces(StatusCodes.Status200OK, typeof(bool))
            .Produces(StatusCodes.Status400BadRequest, typeof(BadRequestResponse))
            .Produces(StatusCodes.Status401Unauthorized, typeof(UnauthorizedResult))
            .Produces(StatusCodes.Status500InternalServerError);
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