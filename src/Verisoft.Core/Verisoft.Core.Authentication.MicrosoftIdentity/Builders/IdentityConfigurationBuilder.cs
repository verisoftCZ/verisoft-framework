using Verisoft.Core.Authentication.MicrosoftIdentity.Configuration;

namespace Verisoft.Core.Authentication.MicrosoftIdentity.Builders
{
    public class IdentityConfigurationBuilder<TUser>
    {
        private JwtTokenGeneratorConfiguration<TUser> jwtConfiguration = new();
        private string defaultEndpointRoute = Constants.DefaultLoginEndpointRoute;

        public IdentityConfigurationBuilder<TUser> WithJwtConfiguration(Action<JwtTokenGeneratorConfigurationBuilder<TUser>> configureJwt)
        {
            var jwtBuilder = new JwtTokenGeneratorConfigurationBuilder<TUser>();
            configureJwt(jwtBuilder);
            jwtConfiguration = jwtBuilder.Build();
            return this;
        }

        public IdentityConfigurationBuilder<TUser> WithDefaultEndpointRoute(string defaultEndpointRoute)
        {
            this.defaultEndpointRoute = defaultEndpointRoute;
            return this;
        }

        public IdentityConfiguration<TUser> Build()
        {
            return new IdentityConfiguration<TUser>
            {
                JwtConfiguration = jwtConfiguration,
                DefaultEndpointRoute = defaultEndpointRoute,
            };
        }
    }
}
