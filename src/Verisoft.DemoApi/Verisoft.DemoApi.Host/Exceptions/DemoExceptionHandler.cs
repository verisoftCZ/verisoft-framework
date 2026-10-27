using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Verisoft.DemoApi.Host.Exceptions;

public sealed class DemoExceptionHandler(
    ILogger<DemoExceptionHandler> logger,
    IProblemDetailsService problemDetailsService)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not DemoException demoException)
        {
            return false;
        }

        logger.LogWarning(
            exception,
            "Demo exception occurred for request {Path}",
            httpContext.Request.Path);

        httpContext.Response.StatusCode = StatusCodes.Status418ImATeapot;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Title = "Demo",
                Detail = demoException.Message,
                Status = StatusCodes.Status418ImATeapot,
                Type = "https://httpstatuses.com/418",
                Extensions =
                {
                    ["example-custom-field-1"] = "these can be simple strings",
                    ["example-custom-field-2"] = new DemoExceptionCustomPayload("or even complex objects", "which get serialized"),
                },
            },
        });
    }

    private record DemoExceptionCustomPayload(string Text1, string Text2);
}