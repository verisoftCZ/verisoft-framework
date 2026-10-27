using Verisoft.Core.Contracts;

namespace Verisoft.Core.Codebooks.Authorization;

public interface ICodebookAuthorizationChecker
{
    Task<HasPermissionResult> HasPermissionAsync(params string[] requiredPermissions);
}
