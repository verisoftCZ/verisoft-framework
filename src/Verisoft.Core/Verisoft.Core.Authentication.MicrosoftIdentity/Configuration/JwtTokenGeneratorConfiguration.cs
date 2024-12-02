namespace Verisoft.Core.Authentication.MicrosoftIdentity.Configuration
{
    public class JwtTokenGeneratorConfiguration<TUser>
    {
        public Dictionary<string, Func<TUser, string>> AdditionClaims { get; set; } = [];
    }
}