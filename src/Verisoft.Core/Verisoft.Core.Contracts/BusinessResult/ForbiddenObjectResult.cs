using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Verisoft.Core.Contracts.BusinessResult;

[DefaultStatusCode(403)]
public class ForbiddenObjectResult(object value) : ObjectResult(value)
{
}
