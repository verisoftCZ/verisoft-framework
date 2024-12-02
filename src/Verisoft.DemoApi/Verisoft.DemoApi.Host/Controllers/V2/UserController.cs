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
[ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
[ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(AuthorizationResult))]
[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Verisoft.Core.Contracts.FailureDetail))]
public class UserController(IUserService userService)
{
    [HttpGet]
    [Route("{id}")]
    [Authorize(Roles = $"{Roles.DemoEdit},{Roles.Admin}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundObjectResult))]
    public async Task<IActionResult> GetUserById(int id)
    {
        var result = await userService.GetUserAsync(id);
        return result.ToActionResult();
    }

    [HttpGet]
    [Route("list")]
    [Authorize(Roles = $"{Roles.DemoEdit},{Roles.Admin}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserListData))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestResponse))]
    public async Task<UserListData> GetUserListAsync([FromQuery] Verisoft.Core.Contracts.FilteredRequest<UserFilter> request)
    {
        return await userService.GetUserListAsync(request);
    }

    [HttpGet]
    [Authorize(Roles = $"{Roles.DemoEdit},{Roles.Admin}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<User>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestResponse))]
    public async Task<IEnumerable<User>> GetUsersAsync([FromQuery] Verisoft.Core.Contracts.FilteredRequest<UserFilter> request)
    {
        return await userService.GetUsersAsync(request);
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.DemoEdit},{Roles.Admin}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestResponse))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(UnprocessableEntity))]
    public async Task<IActionResult> AddUserAsync([FromBody] UserCreateModel userCreateModel)
    {
        var result = await userService.AddUserAsync(userCreateModel);
        return result.ToActionResult();
    }

    [HttpPut]
    [Route("{id}")]
    [Authorize(Roles = $"{Roles.DemoEdit},{Roles.Admin}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundObjectResult))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(UnprocessableEntity))]
    public async Task<IActionResult> UpdateUserAsync([FromBody] UserEditModel userEditModel, int id)
    {
        var result = await userService.UpdateUserAsync(userEditModel, id);
        return result.ToActionResult();
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(Roles = $"{Roles.DemoDelete},{Roles.Admin}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundObjectResult))]
    public async Task<IActionResult> DeleteUserAsync(int id)
    {
        var result = await userService.RemoveUserAsync(id);
        return result.ToActionResult();
    }
}
