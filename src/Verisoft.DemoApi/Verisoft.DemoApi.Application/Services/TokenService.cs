using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Verisoft.Core.Authentication.Permissions;
using Verisoft.DemoApi.Application.Services.Interfaces;
using Verisoft.DemoApi.Contracts.Models.User;

namespace Verisoft.DemoApi.Application.Services;

public class TokenService(IConfiguration configuration, IPermissionService permissionService)
    : ITokenService
{
    private const int TokenValidityInDays = 1;
    private readonly string secret = configuration.GetValue<string>("apiSecret");

    public async Task<string> Generate(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secret);
        var claims = GetClaims(user).ToList();
        claims.AddRange(await GetPermissionClaimsAsync(user));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(TokenValidityInDays),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private async Task<IEnumerable<Claim>> GetPermissionClaimsAsync(User user)
    {
        var claims = new List<Claim>();
        var permissions = await permissionService.GetUserPermissionsAsync(user.Id.ToString());
        foreach (string permission in permissions)
        {
            claims.Add(new(VerisoftClaims.Permission, permission));
        }

        return claims;
    }

    private IEnumerable<Claim> GetClaims(User user)
    {
        yield return new Claim(ClaimTypes.Name, user.Surname);
        yield return new Claim(ClaimTypes.Role, "Admin");
        yield return new Claim(ClaimTypes.NameIdentifier, user.Id.ToString());
        yield return new Claim(ClaimTypes.Email, user.Email);
    }
}
