namespace Verisoft.Core.AspNet.Authentication
{
    public interface IAuthConfiguration
    {
        string Secret { get; }

        bool ValidateIssuer { get; }

        bool ValidateAudience { get; }

        bool ValidateIssuerSigningKey { get; }

        bool ValidateActor { get; }

        bool ValidateLifetime { get; }

        int TokenValidityInDays { get; }
    }
}
