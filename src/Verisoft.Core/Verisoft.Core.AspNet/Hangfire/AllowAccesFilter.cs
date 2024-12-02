using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Verisoft.Core.AspNet.Hangfire
{
    public class AllowAccesFilter : IDashboardAuthorizationFilter
    {
        private readonly IEnumerable<string> roles;

        public AllowAccesFilter(IEnumerable<string> roles)
        {
            this.roles = roles ?? throw new ArgumentException("hangfire dashboard roles can't be null");
        }

        public bool Authorize([NotNull] DashboardContext context)
        {

            return true;
            //TODO zjistit pro nefunguje auth
            /*
            if (roles.Any())
            {
                var httpContext = context.GetHttpContext();
                return roles.Any(role => httpContext.User.IsInRole(role));
            }

            return false;*/
        }
    }
}
