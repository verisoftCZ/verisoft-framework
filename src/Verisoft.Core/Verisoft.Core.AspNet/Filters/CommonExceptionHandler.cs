using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Verisoft.Core.AspNet.ApplicationInsights;
using Verisoft.Core.Common.Logging;
using Verisoft.Core.Contracts;

namespace Verisoft.Core.AspNet.Filters
{
    public static class CommonExceptionHandler
    {
        private const string JsonMime = "application/json";

        internal static async Task HandleAsync(HttpContext context)
        {
            var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            var exception = exceptionHandlerPathFeature.Error;
            var operationId = OperationIdProvider.GetOperationId(context);
            if (exception is BusinessException ex)
            {
                await HandleExceptionAsync(exception, context, FailureDetail.AsBusinessException(operationId, ex.Message));
            }
            else
            {
                await HandleExceptionAsync(exception, context, FailureDetail.AsInternalServerError(operationId));
            }
        }

        private static async Task HandleExceptionAsync(Exception ex, HttpContext context, FailureDetail failureDetail)
        {
            var businessTracker = context.RequestServices.GetService(typeof(IBusinessTracker)) as IBusinessTracker;
            businessTracker?.TrackException(ex);
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var json = JsonConvert.SerializeObject(failureDetail);
            context.Response.ContentType = JsonMime;
            await context.Response.WriteAsync(json);
        }
    }
}
