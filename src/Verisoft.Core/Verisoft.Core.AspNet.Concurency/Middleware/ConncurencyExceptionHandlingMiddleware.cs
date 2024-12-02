using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;

namespace Verisoft.Core.AspNet.Concurency.Middleware;

public class ConncurencyExceptionHandlingMiddleware
{
    private readonly RequestDelegate next;

    public ConncurencyExceptionHandlingMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            await HandleConcurrencyExceptionAsync(context, ex);
        }
    }

    private static Task HandleConcurrencyExceptionAsync(HttpContext context, DbUpdateConcurrencyException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.Conflict;

        string type = exception.Message.Contains("Pesimistic") ? "pessimistic" : "optimistic";

        var response = new
        {
            type,
            detail = exception.Message,
        };

        return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
}
