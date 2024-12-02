namespace Verisoft.Core.Authentication.MicrosoftIdentity.Configuration
{
    public class IdentityConfiguration<TUser> : ILoginEndpointRouteConfiguration
    {
        public JwtTokenGeneratorConfiguration<TUser> JwtConfiguration { get; set; } = new();

        public string DefaultEndpointRoute { get; set; } = Constants.DefaultLoginEndpointRoute;
    }
}
