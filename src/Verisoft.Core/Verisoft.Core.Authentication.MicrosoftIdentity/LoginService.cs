using Microsoft.AspNetCore.Identity;

namespace Verisoft.Core.Authentication.MicrosoftIdentity
{
    internal class LoginService<TUser>(
        UserManager<TUser> userManager,
        SignInManager<TUser> signInManager,
        IIdentityJwtTokenGenerator<TUser> jwtTokenGenerator)
        : ILoginService
        where TUser : IdentityUser
    {
        private readonly UserManager<TUser> userManager = userManager;
        private readonly SignInManager<TUser> signInManager = signInManager;
        private readonly IIdentityJwtTokenGenerator<TUser> jwtTokenGenerator = jwtTokenGenerator;

        public async Task<string?> LoginAsync(string email, string password)
        {
            var result = await signInManager.PasswordSignInAsync(email, password, true, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var user = await userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    var token = await jwtTokenGenerator.GenerateToken(user);
                    return token;
                }
            }

            return null;
        }
    }
}
