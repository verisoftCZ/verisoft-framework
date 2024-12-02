using Verisoft.Core.Contracts;
using Verisoft.Core.Contracts.BusinessResult;
using Verisoft.DemoApi.Contracts.Filters;
using Verisoft.DemoApi.Contracts.Models.User;

namespace Verisoft.DemoApi.Application.Services.Interfaces;

public interface IUserService
{
    Task<BusinessActionResult<User>> AddUserAsync(UserCreateModel userCreateModel);

    Task<BusinessActionResult<User>> GetUserAsync(int id);

    Task<User> GetUserByEmailAsync(string email);

    Task<User> GetUserByApiKeyAsync(string apikey);

    Task<UserListData> GetUserListAsync(FilteredRequest<UserFilter> filter);

    Task<IEnumerable<User>> GetUsersAsync(FilteredRequest<UserFilter> filter);

    Task<BusinessActionResult<User>> RemoveUserAsync(int id);

    Task<BusinessActionResult<User>> UpdateUserAsync(UserEditModel userEditModel, int id);
}
