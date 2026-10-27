using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Verisoft.Core.ApplicationInsights;
using Verisoft.Core.AspNet.Authorization;

namespace Verisoft.Core.AspNet.Filters;

public class ApiHeaderAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public string[]? Roles { get; set; }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var operationId = OperationIdProvider.GetOperationId(context.HttpContext);
        if (!context.HttpContext.Request.Headers.TryGetValue(HeaderNames.Authorization, out var headerValue))
        {
            context.Result = MissingHeaderResult(operationId);
            return;
        }

        var config = context.HttpContext.RequestServices.GetService(typeof(IAuthConfig)) as IAuthConfig;
        var apps = config!.Applications;
        var matchedApp = FindApp(headerValue, apps);
        if (matchedApp == null)
        {
            context.Result = UnknownAppResult(operationId);
            return;
        }

        if (!HasRights(matchedApp))
        {
            context.Result = InsufficientRightsResult(operationId);
        }
    }

    private bool HasRights(Application matchedApp)
    {
        if (Roles == null || Roles.Length == 0)
        {
            return true;
        }

        return matchedApp!.Roles!.Any(x => Roles.Contains(x));
    }

    private Application? FindApp(StringValues headerValue, IEnumerable<Application> apps)
        => apps.FirstOrDefault(x => x.Key == headerValue.ToString());

    private IActionResult UnknownAppResult(string operationId)
        => CreateProblemDetails(
            statusCode: StatusCodes.Status401Unauthorized,
            title: "Unauthorized",
            detail: $"Unknown app or invalid {HeaderNames.Authorization} header.",
            operationId: operationId);

    private IActionResult MissingHeaderResult(string operationId)
        => CreateProblemDetails(
            statusCode: StatusCodes.Status401Unauthorized,
            title: "Unauthorized",
            detail: $"Missing {HeaderNames.Authorization} header.",
            operationId: operationId);

    private IActionResult InsufficientRightsResult(string operationId)
        => CreateProblemDetails(
            statusCode: StatusCodes.Status403Forbidden,
            title: "Forbidden",
            detail: "Insufficient rights.",
            operationId: operationId);

    private IActionResult CreateProblemDetails(
        int statusCode,
        string title,
        string detail,
        string operationId)
    {
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
        };

        problemDetails.Extensions["operationId"] = operationId;

        return new ObjectResult(problemDetails)
        {
            StatusCode = statusCode,
        };
    }
}
