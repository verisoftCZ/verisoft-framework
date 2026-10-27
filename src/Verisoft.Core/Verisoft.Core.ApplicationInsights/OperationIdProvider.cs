using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;

namespace Verisoft.Core.ApplicationInsights
{
    public static class OperationIdProvider
    {
        public static string GetOperationId(HttpContext context)
        {
            var requestTelemetry = context.Features.Get<RequestTelemetry>();
            var operationId = requestTelemetry?.Context?.Operation?.Id ?? requestTelemetry?.Id;
            if (string.IsNullOrEmpty(operationId))
            {
                return $"un-{Guid.NewGuid()}";
            }

            return operationId;
        }
    }
}
