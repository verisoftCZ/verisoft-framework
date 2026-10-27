namespace Verisoft.Core.Authentication;

public static class HeaderConstants
{
    public const string AuthorizationTokenBearerPrefix = $"{BearerAuthenticationType} ";
    public const string BearerAuthenticationType = "Bearer";
    public const string AuthorizationHeader = "Authorization";
}