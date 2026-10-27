using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Verisoft.Core.Authentication.Permissions;
using Verisoft.DemoApi.Application.Services.Interfaces;
using Verisoft.DemoApi.Contracts.Models.User;

namespace Verisoft.DemoApi.Application.Services;

public class TokenService(IConfiguration configuration, IPermissionService permissionService)
    : ITokenService
{
    private const int TokenValidityInDays = 1;
    private readonly byte[] signingKey = GetValidatedSigningKey(configuration);

    public async Task<string> Generate(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = GetClaims(user).ToList();
        claims.AddRange(await GetPermissionClaimsAsync(user));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(TokenValidityInDays),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(signingKey), SecurityAlgorithms.HmacSha256Signature),
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private static byte[] GetValidatedSigningKey(IConfiguration configuration)
    {
        var secret = configuration.GetValue<string>("apiSecret");
        if (string.IsNullOrWhiteSpace(secret))
        {
            throw new InvalidOperationException("Configuration value 'apiSecret' is missing or empty.");
        }

        var key = Encoding.UTF8.GetBytes(secret);
        if (key.Length <= 32)
        {
            throw new InvalidOperationException("Configuration value 'apiSecret' must be longer than 32 bytes for HS256 signing.");
        }

        return key;
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
        if (user.Surname is not null)
        {
            yield return new Claim(ClaimTypes.Name, user.Surname);
        }

        yield return new Claim(ClaimTypes.Role, "Admin");
        yield return new Claim(ClaimTypes.NameIdentifier, user.Id.ToString());

        if (user.Email is not null)
        {
            yield return new Claim(ClaimTypes.Email, user.Email);
        }
    }
}
