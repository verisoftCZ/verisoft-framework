using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Verisoft.Core.AspNet.Authentication;
using Verisoft.Core.Authentication.MicrosoftIdentity.Configuration;
using Verisoft.Core.Authentication.Permissions;

namespace Verisoft.Core.Authentication.MicrosoftIdentity;

public class JwtTokenGenerator<TUser>
    (JwtTokenGeneratorConfiguration<TUser> config,
    UserClaimsPrincipalFactory<TUser, IdentityRole> claimsPrincipalFactory,
    IAuthConfiguration authConfiguration,
    IPermissionService permissionService)
    : IIdentityJwtTokenGenerator<TUser>
    where TUser : IdentityUser
{
    private readonly UserClaimsPrincipalFactory<TUser, IdentityRole> claimsPrincipalFactory = claimsPrincipalFactory ?? throw new ArgumentNullException(nameof(claimsPrincipalFactory));
    private readonly JwtTokenGeneratorConfiguration<TUser> config = config ?? throw new ArgumentNullException(nameof(config));
    private readonly IAuthConfiguration authConfiguration = authConfiguration ?? throw new ArgumentNullException(nameof(authConfiguration));
    private readonly IPermissionService permissionService = permissionService ?? throw new ArgumentNullException(nameof(permissionService));

    public async Task<string> GenerateToken(TUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(authConfiguration.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(await GetClaims(user)),
            Expires = DateTime.UtcNow.AddDays(authConfiguration.TokenValidityInDays),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private async Task<IEnumerable<Claim>> GetClaims(TUser user)
    {
        var claims = new List<Claim>();
        var principal = await claimsPrincipalFactory.CreateAsync(user);
        claims.AddRange(principal.Claims);

        foreach (var claimDefinition in config.AdditionClaims)
        {
            claims.Add(new Claim(claimDefinition.Key, claimDefinition.Value.Invoke(user)));
        }

        var permissions = await permissionService
            .GetUserPermissionsAsync(user.Id);

        foreach (string permission in permissions)
        {
            claims.Add(new (VerisoftClaims.Permission, permission));
        }

        return claims;
    }
}
