using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Verisoft.Core.ConfigurationStorage;

[ApiController]
[Authorize]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
public abstract class ConfigurationStorageControllerBase(IConfigurationStorage storage) : ControllerBase
{
    [HttpGet("{componentId}/{configurationType}")]
    [ProducesResponseType(typeof(ComponentConfigurationsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<ComponentConfigurationsResponse>> Get(string componentId, string configurationType, CancellationToken cancellationToken)
    {
        var result = await storage.GetAsync([Key(componentId, configurationType)], cancellationToken);
        return Ok(result[0]);
    }

    [HttpPost("query")]
    [ProducesResponseType(typeof(IReadOnlyList<ComponentConfigurationsResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ComponentConfigurationsResponse>>> Query(ConfigurationBatchRequest request, CancellationToken cancellationToken)
        => Ok(await storage.GetAsync(request.Configurations, cancellationToken));

    [HttpPut("{componentId}/{configurationType}/user")]
    [ProducesResponseType(typeof(ConfigurationResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<ConfigurationResponse>> SaveUser(string componentId, string configurationType, SaveConfigurationRequest request, CancellationToken cancellationToken)
        => Ok(await storage.UpsertAsync(Key(componentId, configurationType), ConfigurationLevel.User, request, cancellationToken));

    [HttpPut("{componentId}/{configurationType}/admin")]
    [Authorize(Policy = ConfigurationStorageOptions.AdminPolicy)]
    [ProducesResponseType(typeof(ConfigurationResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<ConfigurationResponse>> SaveAdmin(string componentId, string configurationType, SaveConfigurationRequest request, CancellationToken cancellationToken)
        => Ok(await storage.UpsertAsync(Key(componentId, configurationType), ConfigurationLevel.Admin, request, cancellationToken));

    [HttpDelete("{componentId}/{configurationType}/user")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser(string componentId, string configurationType, CancellationToken cancellationToken)
        => await storage.DeleteAsync(Key(componentId, configurationType), ConfigurationLevel.User, cancellationToken) ? NoContent() : NotFound();

    [HttpDelete("{componentId}/{configurationType}/admin")]
    [Authorize(Policy = ConfigurationStorageOptions.AdminPolicy)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAdmin(string componentId, string configurationType, CancellationToken cancellationToken)
        => await storage.DeleteAsync(Key(componentId, configurationType), ConfigurationLevel.Admin, cancellationToken) ? NoContent() : NotFound();

    private static ConfigurationKey Key(string componentId, string configurationType)
        => new() { ComponentId = componentId, ConfigurationType = configurationType };
}
