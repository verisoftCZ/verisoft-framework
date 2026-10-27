namespace Verisoft.Core.Codebooks.Common.LanguageServices;

public interface ILanguageProvider
{
    string? PreferredLanguageCode { get; }

    static string DefaultLanguageCode => "Default";
}