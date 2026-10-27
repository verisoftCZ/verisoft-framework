namespace Verisoft.Core.Codebooks.Configurations;

public class CodebookAuthorizationConfiguration
{
    public const string SectionName = "CodebookAuthorization";

    public IEnumerable<string> CanEditCodebook { get; set; } = [];
}
