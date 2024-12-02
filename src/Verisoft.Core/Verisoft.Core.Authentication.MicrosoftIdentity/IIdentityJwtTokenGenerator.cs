using Microsoft.AspNetCore.Identity;

namespace Verisoft.Core.Authentication.MicrosoftIdentity
{
    internal interface IIdentityJwtTokenGenerator<TUser>
        where TUser : IdentityUser
    {
        Task<string> GenerateToken(TUser user);
    }
}
