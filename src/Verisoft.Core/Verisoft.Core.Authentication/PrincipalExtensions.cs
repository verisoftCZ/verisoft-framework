using System;
using System.Security.Claims;
using System.Security.Principal;

namespace Verisoft.Core.Authentication
{
    public static class PrincipalExtensions
    {
        public static string? FirstName(this IPrincipal principal)
        {
            return GetClaimByName(principal, ClaimTypes.GivenName);
        }

        public static string? GetSurname(this IPrincipal principal)
        {
            return GetClaimByName(principal, ClaimTypes.Surname);
        }

        public static string GetUserName(this IPrincipal principal)
        {
            return GetClaimByName(principal, ClaimTypes.NameIdentifier) ?? principal?.Identity?.Name ?? string.Empty;
        }

        /// <summary>
        /// Gets the authenticated user's identifier from the specified claim without falling back to a name.
        /// </summary>
        /// <remarks>
        /// GetUserName can fall back to Identity.Name, and HttpUserContext uses Identity.Name for auditing.
        /// A name may be unique in an application, but these APIs do not guarantee a stable, unique ownership key.
        /// Use a claim whose issuer guarantees that contract; its value is returned unchanged.
        /// </remarks>
        /// <param name="principal">The authenticated principal.</param>
        /// <param name="claimType">The identifier claim type; defaults to ClaimTypes.NameIdentifier.</param>
        /// <returns>The nonblank identifier claim value.</returns>
        /// <exception cref="ArgumentException">The claim type is blank.</exception>
        /// <exception cref="UnauthorizedAccessException">The principal is not authenticated or the identifier claim is missing or blank.</exception>
        public static string GetUserId(this IPrincipal principal, string claimType = ClaimTypes.NameIdentifier)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(claimType);
            if (principal is not ClaimsPrincipal { Identity.IsAuthenticated: true })
            {
                throw new UnauthorizedAccessException("An authenticated identity is required.");
            }

            var userId = GetClaimByName(principal, claimType);
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedAccessException("A stable user identifier is required.");
            }

            return userId;
        }

        private static string? GetClaimByName(IPrincipal principal, string name)
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
