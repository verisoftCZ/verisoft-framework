using Verisoft.Core.Authentication.MicrosoftIdentity.Configuration;

namespace Verisoft.Core.Authentication.MicrosoftIdentity.Builders
{
    public class JwtTokenGeneratorConfigurationBuilder<TUser>
    {
        private Dictionary<string, Func<TUser, string>> additionalClaims = new();

        public JwtTokenGeneratorConfigurationBuilder<TUser> WithAdditionalClaims(string key, Func<TUser, string> claimFunc)
        {
            additionalClaims[key] = claimFunc;
            return this;
        }

        public JwtTokenGeneratorConfiguration<TUser> Build()
        {
            return new JwtTokenGeneratorConfiguration<TUser>
            {
                AdditionClaims = additionalClaims,
            };
        }
    }
}
