using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Verisoft.Core.Common.Exceptions;

namespace Verisoft.Core.AspNet.Exceptions;

public sealed class InvalidSortPropertyExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not InvalidSortPropertyException invalidSortPropertyException)
        {
            return false;
        }

        httpContext.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status422UnprocessableEntity,
            Title = "Invalid sort property",
            Detail = invalidSortPropertyException.Message,
            Type = "https://httpstatuses.com/422",
            Extensions =
            {
                ["errorType"] = "invalid_sort_property",
            },
        };

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);

        return true;
    }
}
