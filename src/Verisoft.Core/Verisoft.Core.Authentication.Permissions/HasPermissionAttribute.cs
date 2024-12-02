using Microsoft.AspNetCore.Authorization;

namespace Verisoft.Core.Authentication.Permissions
{
    public sealed class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(object permission)
            : base(policy: permission.ToString())
        {

        }
    }
}
