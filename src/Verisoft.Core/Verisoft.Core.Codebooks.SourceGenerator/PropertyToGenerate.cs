namespace Verisoft.Core.Codebooks.SourceGenerator;

public class PropertyToGenerate(
    string originalPropertyName,
    string foreignKeyPropertyName,
    string navigationPropertyName,
    string codebookTypeArgumentFullName,
    bool isNullable)
{
    public string OriginalPropertyName { get; } = originalPropertyName;

    public string ForeinKeyPropertyName { get; } = foreignKeyPropertyName;

    public string NavigationPropertyName { get; } = navigationPropertyName;

    public string CodebookTypeArgumentFullName { get; } = codebookTypeArgumentFullName;

    public bool IsNullable { get; } = isNullable;
}