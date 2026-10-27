using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Verisoft.Core.AspNet.Concurency.Exceptions;

public sealed class ConcurrencyExceptionHandler(
    ILogger<ConcurrencyExceptionHandler> logger,
    IProblemDetailsService problemDetailsService)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not DbUpdateConcurrencyException concurrencyException)
        {
            return false;
        }

        logger.LogWarning(
            concurrencyException,
            "Concurrency conflict occurred for request {Path}",
            httpContext.Request.Path);

        var concurrencyType =
            concurrencyException.Message.Contains(
                "Pessimistic",
                StringComparison.OrdinalIgnoreCase)
                ? "pessimistic"
                : "optimistic";

        httpContext.Response.StatusCode = StatusCodes.Status409Conflict;

        return await problemDetailsService.TryWriteAsync(
            new ProblemDetailsContext
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = new ProblemDetails
                {
                    Title = "Concurrency conflict",
                    Detail = concurrencyException.Message,
                    Status = StatusCodes.Status409Conflict,
                    Type = "https://httpstatuses.com/409",
                    Extensions =
                    {
                        ["concurrencyType"] = concurrencyType,
                    },
                },
            });
    }
}