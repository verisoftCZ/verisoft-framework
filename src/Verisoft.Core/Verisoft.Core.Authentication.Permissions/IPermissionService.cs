namespace Verisoft.Core.Authentication.Permissions;

public interface IPermissionService
{
    Task<IEnumerable<string>> GetUserPermissionsAsync(string userId);
}
