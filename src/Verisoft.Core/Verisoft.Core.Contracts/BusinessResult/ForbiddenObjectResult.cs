using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Verisoft.Core.Contracts.BusinessResult;

[DefaultStatusCode(StatusCodes.Status403Forbidden)]
public class ForbiddenObjectResult : ObjectResult
{
    public ForbiddenObjectResult(object value)
        : base(value)
    {
        StatusCode = StatusCodes.Status403Forbidden;
    }
}
