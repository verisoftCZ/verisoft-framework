using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Verisoft.Core.AspNet.Mvc;
using Verisoft.Core.Contracts.BusinessResult;
using Verisoft.DemoApi.Application.Services.Interfaces;
using Verisoft.DemoApi.Common.Enums;
using Verisoft.DemoApi.Contracts.Filters;
using Verisoft.DemoApi.Contracts.Models.User;

namespace Verisoft.DemoApi.Host.Controllers.V2;

[AllowAnonymous]
[ApiController]
[ApiVersion("2.0")]
[Route("v{v:apiVersion}/[controller]")]
[ProducesResponseType<UnauthorizedResult>(StatusCodes.Status401Unauthorized)]
[ProducesResponseType<AuthorizationResult>(StatusCodes.Status403Forbidden)]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
public class UserController(IUserService userService)
{
    [HttpGet]
    [Route("{id}")]
    [Authorize(Roles = $"{Roles.DemoEdit},{Roles.Admin}")]
    [ProducesResponseType<User>(StatusCodes.Status200OK)]
    [ProducesResponseType<NotFoundObjectResult>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult?> GetUserById(int id)
    {
        var result = await userService.GetUserAsync(id);
        return result.ToActionResult();
    }

    [HttpGet]
    [Route("list")]
    [Authorize(Roles = $"{Roles.DemoEdit},{Roles.Admin}")]
    [ProducesResponseType<UserListData>(StatusCodes.Status200OK)]
    [ProducesResponseType<BadRequestResponse>(StatusCodes.Status400BadRequest)]
    public async Task<UserListData?> GetUserListAsync([FromQuery] Verisoft.Core.Contracts.FilteredRequest<UserFilter> request)
    {
        return await userService.GetUserListAsync(request);
    }

    [HttpGet]
    [Authorize(Roles = $"{Roles.DemoEdit},{Roles.Admin}")]
    [ProducesResponseType<IEnumerable<User>>(StatusCodes.Status200OK)]
    [ProducesResponseType<BadRequestResponse>(StatusCodes.Status400BadRequest)]
    public async Task<IEnumerable<User>?> GetUsersAsync([FromQuery] Verisoft.Core.Contracts.FilteredRequest<UserFilter> request)
    {
        return await userService.GetUsersAsync(request);
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.DemoEdit},{Roles.Admin}")]
    [ProducesResponseType<User>(StatusCodes.Status200OK)]
    [ProducesResponseType<BadRequestResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<UnprocessableEntity>(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult?> AddUserAsync([FromBody] UserCreateModel userCreateModel)
    {
        var result = await userService.AddUserAsync(userCreateModel);
        return result.ToActionResult();
    }

    [HttpPut]
    [Route("{id}")]
    [Authorize(Roles = $"{Roles.DemoEdit},{Roles.Admin}")]
    [ProducesResponseType<User>(StatusCodes.Status200OK)]
    [ProducesResponseType<BadRequestResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<NotFoundObjectResult>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<UnprocessableEntity>(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult?> UpdateUserAsync([FromBody] UserEditModel userEditModel, int id)
    {
        var result = await userService.UpdateUserAsync(userEditModel, id);
        return result.ToActionResult();
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(Roles = $"{Roles.DemoDelete},{Roles.Admin}")]
    [ProducesResponseType<User>(StatusCodes.Status200OK)]
    [ProducesResponseType<NotFoundObjectResult>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult?> DeleteUserAsync(int id)
    {
        var result = await userService.RemoveUserAsync(id);
        return result.ToActionResult();
    }
}
