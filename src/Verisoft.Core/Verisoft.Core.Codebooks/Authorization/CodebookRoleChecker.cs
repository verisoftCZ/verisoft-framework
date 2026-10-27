using Verisoft.Core.Authentication;
using Verisoft.Core.Contracts;
using Verisoft.Core.Contracts.BusinessResult;

namespace Verisoft.Core.Codebooks.Authorization;

public class CodebookRoleChecker(IUserContext userContext) : ICodebookAuthorizationChecker
{
    public async Task<HasPermissionResult> HasPermissionAsync(params string[] requiredPermissions)
        => HasPermission(requiredPermissions)
            ? new HasPermissionResult()
            : ErrorFactory.Forbidden(userContext.GetUserName(), PermissionCheckMode.Any, requiredPermissions);

    private bool HasPermission(params string[] requiredPermissions)
        => requiredPermissions.Any(userContext.User.IsInRole);
}
