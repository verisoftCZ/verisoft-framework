using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Verisoft.Core.Data.EntityFramework.Walkers;

namespace Verisoft.Core.AspNet.Exceptions;

public sealed class CrawlerExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not EntityDeleteBlockedException deleteBlockedException)
        {
            return false;
        }

        httpContext.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status422UnprocessableEntity,
            Title = "Delete operation blocked",
            Detail = deleteBlockedException.Message,
            Type = "https://httpstatuses.com/422",
            Extensions =
            {
                ["errorType"] = "restrict_constraint",
            },
        };

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);

        return true;
    }
}
