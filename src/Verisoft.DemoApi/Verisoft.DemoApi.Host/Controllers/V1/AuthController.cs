using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Verisoft.DemoApi.Application.Services.Interfaces;
using Verisoft.DemoApi.Contracts.Models.Auth;

namespace Verisoft.DemoApi.Host.Controllers.V1;

[ApiController]
[Route("[controller]")]
[ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
[ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(AuthorizationResult))]
[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Verisoft.Core.Contracts.FailureDetail))]
public class AuthController(ITokenService tokenService, IUserService personService, ILogger<AuthController> logger) : ControllerBase
{
    private readonly ITokenService tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
    private readonly IUserService userService = personService ?? throw new ArgumentNullException(nameof(personService));
    private readonly ILogger<AuthController> logger = logger ?? throw new ArgumentNullException(nameof(logger));

    [HttpGet("token")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticatedResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    public async Task<IActionResult> Token([FromQuery] string email)
    {
        var user = await userService.GetUserByEmailAsync(email);
        if (user == null)
        {
            return new UnauthorizedResult();
        }

        var token = await tokenService.Generate(user);
        var result = new AuthenticatedResponse()
        {
            Token = token,
        };

        return Ok(result);
    }

    [HttpGet("apikeytoken")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticatedResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    public async Task<IActionResult> ApiKeyToken([FromQuery] string apikey)
    {
        try
        {
            var user = await userService.GetUserByApiKeyAsync(apikey);
            if (user == null)
            {
                logger.LogWarning("No user found for apikey: {ApiKey}", apikey);
                return new UnauthorizedResult();
            }

            var token = await tokenService.Generate(user);
            var result = new AuthenticatedResponse()
            {
                Token = token,
            };

            logger.LogInformation("A token was generated for apikey: {ApiKey}", apikey);

            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error when generating token for apikey: {ApiKey}", apikey);
            throw;
        }
    }
}