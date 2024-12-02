using Verisoft.Core.Authentication.Permissions;
using Verisoft.DemoApi.Common.Enums;

namespace Verisoft.DemoApi.Application.Services;

public class PermissionService : IPermissionService
{
    public async Task<IEnumerable<string>> GetUserPermissionsAsync(string userId)
    {
        await Task.Yield();
        return new HashSet<string> { DemoApiPermissions.ReadClient.ToString() };
    }
}