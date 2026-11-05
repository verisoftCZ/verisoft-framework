using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Verisoft.Core.AspNet.Exceptions;

public sealed class ValidationExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ValidationException validation)
        {
            return false;
        }

        var errors = validation.ValidationResult?.MemberNames.Distinct().DefaultIfEmpty(string.Empty)
            .ToDictionary(member => member, _ => new[] { validation.Message })
            ?? new() { [string.Empty] = new[] { validation.Message } };
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        var problemDetails = new ValidationProblemDetails(errors)
        {
            Status = StatusCodes.Status400BadRequest,
        };
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}