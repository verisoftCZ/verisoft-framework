using System.Security.Claims;
using System.Security.Principal;

namespace Verisoft.Core.Authentication
{
    public static class PrincipalExtensions
    {
        public static string FirstName(this IPrincipal principal)
        {
            return GetClaimByName(principal, ClaimTypes.GivenName);
        }

        public static string GetSurname(this IPrincipal principal)
        {
            return GetClaimByName(principal, ClaimTypes.Surname);
        }

        public static string GetUserName(this IPrincipal principal)
        {
            return GetClaimByName(principal, ClaimTypes.NameIdentifier) ?? principal?.Identity?.Name ?? string.Empty;
        }

        private static string GetClaimByName(IPrincipal principal, string name)
        {
            if (principal == null)
            {
                return null;
            }

            var claimPrincipal = principal as ClaimsPrincipal;
            var claim = claimPrincipal?.FindFirst(name);
            return claim?.Value;
        }
    }
}
