using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Collections.Generic;
using System.Linq;

namespace Verisoft.Core.Contracts.BusinessResult;

public static class BusinessActionResultExtensions
{
    public static IActionResult ToActionResult<T>(
       this BusinessActionResult<T> result,
       IDictionary<string, IActionResult> customCases = null)
    {
        if (result == null)
        {
            return null;
        }

        if (result.IsSuccess)
        {
            return result.Data != null ? new OkObjectResult(result.Data) : new OkResult();
        }

        return GetErrorResult(result, customCases);
    }

    public static IActionResult ToActionResult(
       this BusinessActionResult result,
       IDictionary<string, IActionResult> customCases = null)
    {
        if (result == null)
        {
            return null;
        }

        return result.IsSuccess ? new OkResult() : GetErrorResult(result, customCases);
    }

    private static IActionResult GetErrorResult<T>(BusinessActionResult<T> result, IDictionary<string, IActionResult> customCases)
    {
        var switchCases = GetSwitchCases(result, customCases);
        return switchCases.TryGetValue(result.Code, out var actionResult)
            ? actionResult
            : result.Data != null
                ? new UnprocessableEntityObjectResult(new BusinessActionErrors<T>(result.Data, result.Errors)) { ContentTypes = HttpProblemDetailsStandard() }
                : new UnprocessableEntityObjectResult(result.Errors) { ContentTypes = HttpProblemDetailsStandard() };
    }

    private static IActionResult GetErrorResult(BusinessActionResult result, IDictionary<string, IActionResult> customCases)
    {
        var switchCases = GetSwitchCases(result, customCases);

        return switchCases.TryGetValue(result.Code, out var actionResult)
            ? actionResult
            : new UnprocessableEntityObjectResult(result.Errors) { ContentTypes = HttpProblemDetailsStandard() };
    }

    private static Dictionary<string, IActionResult> GetSwitchCases(BusinessActionResult result, IDictionary<string, IActionResult> customCases)
    {
        var defaultCases = new Dictionary<string, IActionResult>
        {
                { ErrorFactory.NotFoundErrorCode, new NotFoundObjectResult(result.Errors) { ContentTypes = HttpProblemDetailsStandard() } },
                { ErrorFactory.ForbiddenErrorCode, new ForbiddenObjectResult(result.Errors) { ContentTypes = HttpProblemDetailsStandard() } },
                { ErrorFactory.BadRequestErrorCode, new BadRequestObjectResult(result.Errors) { ContentTypes = HttpProblemDetailsStandard() } },
                { ErrorFactory.NullInputErrorCode, new BadRequestObjectResult(result.Errors) { ContentTypes = HttpProblemDetailsStandard() } },
        };

        var switchCases = customCases != null
            ? defaultCases.Concat(customCases).ToDictionary(kv => kv.Key, kv => kv.Value)
            : defaultCases;
        return switchCases;
    }

    private static MediaTypeCollection HttpProblemDetailsStandard() => ["application/problem+json"];
}
