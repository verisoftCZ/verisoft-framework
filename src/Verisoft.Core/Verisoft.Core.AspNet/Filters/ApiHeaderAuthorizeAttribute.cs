using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Verisoft.Core.AspNet.ApplicationInsights;
using Verisoft.Core.AspNet.Authorization;
using Verisoft.Core.Contracts;

namespace Verisoft.Core.AspNet.Filters
{
    public class ApiHeaderAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public string[] Roles { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var operationId = OperationIdProvider.GetOperationId(context.HttpContext);
            if (!context.HttpContext.Request.Headers.TryGetValue(HeaderNames.Authorization, out var headerValue))
            {
                context.Result = MissingHeaderResult(operationId);
                return;
            }

            var config = context.HttpContext.RequestServices.GetService(typeof(IAuthConfig)) as IAuthConfig;
            var apps = config.Applications;
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

            return matchedApp.Roles.Any(x => Roles.Contains(x));
        }

        private Application FindApp(StringValues headerValue, IEnumerable<Application> apps)
            => apps.FirstOrDefault(x => x.Key == headerValue.ToString());

        private IActionResult UnknownAppResult(string operationId) => FromError(
            new FailureDetail
            {
                OperationId = operationId,
                ErrorMessage = $"Unknown app or invalid {HeaderNames.Authorization} header.",
            },
            StatusCodes.Status401Unauthorized);

        private IActionResult MissingHeaderResult(string operationId) => FromError(
            new FailureDetail
            {
                OperationId = operationId,
                ErrorMessage = $"Missing {HeaderNames.Authorization} header.",
            },
            StatusCodes.Status401Unauthorized);

        private IActionResult InsufficientRightsResult(string operationId) => FromError(
            new FailureDetail
            {
                OperationId = operationId,
                ErrorMessage = $"Insufficient rights",
            },
            StatusCodes.Status403Forbidden);

        private IActionResult FromError(FailureDetail error, int statusCode) =>
            new ObjectResult(error) { StatusCode = statusCode };
    }
}
