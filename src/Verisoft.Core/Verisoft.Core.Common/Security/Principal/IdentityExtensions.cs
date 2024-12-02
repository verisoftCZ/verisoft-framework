using System;
using System.Security.Principal;

namespace Verisoft.Core.Common.Security.Principal
{
    public static class IdentityExtensions
    {
        public static string GetDomain(this IIdentity identity)
        {
            var identityName = identity?.Name;
            if (identityName == null)
            {
                return string.Empty;
            }

            var stop = identityName.IndexOf("\\", StringComparison.InvariantCulture);
            return (stop > -1) ? identityName.Substring(0, stop) : string.Empty;
        }

        public static string GetLogin(this IIdentity identity)
        {
            var identityName = identity?.Name;
            if (identityName == null)
            {
                return string.Empty;
            }

            var stop = identityName.IndexOf("\\", StringComparison.InvariantCulture);
            return (stop > -1) ? identityName.Substring(stop + 1, identityName.Length - stop - 1) : string.Empty;
        }
    }
}
