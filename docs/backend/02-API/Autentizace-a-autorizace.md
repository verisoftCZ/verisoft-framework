# Úvod

Pro zabezpečení mikroservis/aplikací pouíváme JWT token.
JTW token obsahuje jednotlivé `Permissions`, které má uživatel přiřazené. Pro jednouducho správu jsou Permissions zgrupované pod `Roles`.

Uživatelům můžu přiřadit pouze `Role` a z těch se pak sestaví seznam jejich permissions.

# Autentizace uživatele
Uživatel je autentizován pomocí JWT tokenu.
JWT token je ověřován pomocí `SymmetricSecurityKey`

Registrace autentizace v aplikaci:

``` csharp
private static void RegisterAuthentication(IServiceCollection serviceCollection, IConfiguration configuration)
{
    serviceCollection.AddAuthorization(c =>
    {
        c.DefaultPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme).Build();
    });

    serviceCollection.AddVerisoftAuth(c => { c.Secret = configuration.GetValue<string>("apiSecret"); });
    serviceCollection.AddSingleton<ITokenService, TokenService>();  //Generuje Token - optional
    serviceCollection.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();  //Permission based access
    serviceCollection.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();  //Permision based policy
}
```

Krom `Secret`, kterým je JWT token ověřen, je také možné nastavit další vlastnosti při ověřování tokenu.
Tyto vlastnosti (properties) jsou v objektu `Verisoft.Core.AspNet.Authentication.IdentityAuthenticationOptions`.

V Program.cs
`app.UseAuthentication();`


# Autorizace uživatele
Pro autorizaci uživatele je nutné na controlleru ověřit jestli má uživatel právo daný controller volat:
`[HasPermission(Permission.ReadClient)]`

Příklad metody:
``` csharp
[HttpGet]
[Route("{id}")]
[HasPermission(DemoApiPermissions.ReadClient)]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Client))]
[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFound))]
public async Task<IActionResult> GetClientAsync(int id)
{
    var result = await clientService.GetClientAsync(id);
    return result.ToActionResult();
}
```

V contractech si vytvoříme enum:

``` csharp
public enum DemoApiPermissions
{
    None = 0,
    ReadClient = 1,
    UpdateClient = 2,
    DeleteClient = 3,
}
```

V Program.cs
`app.UseAuthorization();`


# Jak si vygenerovat JWT Token:
Je nutné implementovat `ITokenService`

Nejjednodušší příklad implementace pro testování je v:
`Verisoft.DemoApi.Application.Services.TokenService`

Tato metoda slouží pouze jako příklad v real-case scenario je nutné načítat permissions a role z DB nebo API. A to konkrétně v `IPermissionService`.
``` csharp
public class TokenService : ITokenService
{
    private const int TokenValidityInDays = 1;
    private readonly string secret;
    private readonly IPermissionService permissionService;

    public TokenService(IConfiguration configuration, IPermissionService permissionService)
    {
        this.secret = configuration.GetValue<string>("apiSecret");
        this.permissionService = permissionService;
    }

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

    private static IEnumerable<Claim> GetClaims(User user)
    {
        yield return new Claim(ClaimTypes.Name, user.Surname);
        yield return new Claim(ClaimTypes.NameIdentifier, user.Id.ToString());
        yield return new Claim(ClaimTypes.Email, user.Email);
    }

    private async Task<List<Claim>> GetPermissionClaimsAsync(User user)
    {
        var claims = new List<Claim>();
        var permissions = await permissionService.GetPermissionsAsync(user.Id.ToString());
        foreach (string permission in permissions)
        {
            claims.Add(new(VerisoftClaims.Permissions, permission));
        }

        return claims;
    }
}
```

# Servisní účty/3rd párty
Pro servisní účty (například CRON joby, které nemají uživatele) nebo pro aplikace 3tích stran použijeme API key.
Identity API si drží seznam API, klíčů, které může revokovat pokud už nějsou platné, nebo došlo k jejich kompromitaci.
Pomocí tohoto API klíče získá CRON job nebo 3tí strana JWT token s přednastavenými permissions a pak už je flow stejné jako u Autorizace.

## Jak zavolat Identity API pro vygenerování JWT klíče s API klíčem
URL si upravte dle prostředí ve kterém pracujete

``` bash
curl -X 'GET' \
  'https://localhost:63088/api/Auth/apikeytoken?apikey=xxx' \
  -H 'accept: text/plain'
```
Response:

``` js
{
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkhhbmNvY2siLCJyb2xlIjoiQWRtaW4iLCJuYW1laWQiOiIxIiwiZW1haWwiOiJwYndpaXJsLnptYWFzZG1xb0BtZmVib3dxby5oenF1ZmYubmV0IiwicGVybWlzc2lvbiI6IlJlYWRDbGllbnQiLCJuYmYiOjE3MzEzNTQ4OTIsImV4cCI6MTczMTQ0MTI5MiwiaWF0IjoxNzMxMzU0ODkyfQ.vWljHXbR0BPEMj4OxpDgLVquG3oy1bDpvmfzNSb4HvE"
}
```
