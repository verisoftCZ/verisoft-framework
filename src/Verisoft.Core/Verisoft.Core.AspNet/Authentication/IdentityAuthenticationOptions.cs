namespace Verisoft.Core.AspNet.Authentication
{
    public class IdentityAuthenticationOptions : IAuthConfiguration
    {
        public string Secret { get; set; } = string.Empty;

        public bool ValidateIssuer { get; set; } = true;

        public bool ValidateAudience { get; set; } = true;

        public bool ValidateIssuerSigningKey { get; set; } = true;

        public bool ValidateActor { get; set; } = true;

        public bool ValidateLifetime { get; set; } = true;

        public int TokenValidityInDays { get; set; } = 1;
    }
}
