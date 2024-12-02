using Microsoft.AspNetCore.Authorization;

namespace Verisoft.Core.Authentication.Permissions
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            var permissions = context
                .User
                .Claims
                .Where(x => x.Type == VerisoftClaims.Permission)
                .Select(x => x.Value)
                .ToHashSet();

            if (permissions.Contains(requirement.Permission))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
