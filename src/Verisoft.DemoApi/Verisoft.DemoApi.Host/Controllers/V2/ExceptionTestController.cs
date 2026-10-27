using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Verisoft.Core.AspNet.Exceptions;
using Verisoft.DemoApi.Host.Exceptions;

namespace Verisoft.DemoApi.Host.Controllers.V2;

[ApiController]
[AllowAnonymous]
[ApiVersion("2.0")]
[Route("v{v:apiVersion}/[controller]")]
[ProducesResponseType<UnauthorizedResult>(StatusCodes.Status401Unauthorized)]
[ProducesResponseType<AuthorizationResult>(StatusCodes.Status403Forbidden)]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
public sealed class ExceptionTestController() : ControllerBase
{
    /// <summary>
    /// Throws an unhandled exception, expecting to be caught by <see cref="GlobalExceptionHandler"/>
    /// </summary>
    [HttpGet("exception")]
    public async Task<IActionResult> ThrowUnhandledAsync()
    {
        throw new Exception();
    }

    /// <summary>
    /// Throws a demo exception, expecting to be caught by <see cref="DemoExceptionHandler"/>
    /// </summary>
    [HttpGet("demo")]
    public async Task<IActionResult> ThrowDemoAsync()
    {
        throw new DemoException("This exception was intentionally thrown for purposes of testing the IExceptionHandler pipeline");
    }
}
