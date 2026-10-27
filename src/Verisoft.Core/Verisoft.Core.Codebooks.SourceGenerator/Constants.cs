namespace Verisoft.Core.Codebooks.SourceGenerator;

public static class Constants
{
    public const string CodebookAttributeFullName = "Verisoft.Core.Codebooks.Common.Attributes.GenerateCodebookInfrastructureAttribute";

    public const string CodebookRecordTypeName = "CodebookRecord";
    public const string CodebookRecordForeignKeyPropertySuffix = "StringValue";
    public const string CodebookRecordNavigationPropertySuffix = "CodebookRecord";

    public const string CodebookRecordEntityTypeName = "CodebookRecordEntity";
    public const string CodebookRecordEntityNamespace = "Verisoft.Core.Common.Entities.Codebooks";

    public const string CodebookInfrastructureFileNameSuffix = "CodebookInfrastructure.g.cs";

    public const string PragmaWarningDisable = "#pragma warning disable";
    public const string PragmaWarningRestore = "#pragma warning restore";
    public const string NotUsedWarningCode = "CS0169";
    public const string NullableWarningCode = "CS8618";

    public const string Nullable = "?";
    public const string SummaryStart = "/// <summary>";
    public const string SummaryEnd = "/// </summary>";
}