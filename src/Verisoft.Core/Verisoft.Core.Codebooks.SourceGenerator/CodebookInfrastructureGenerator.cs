using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Verisoft.Core.Codebooks.SourceGenerator;

[Generator]
public class CodebookInfrastructureGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<PropertyDeclarationSyntax> propertyDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (syntaxNode, _) => syntaxNode is PropertyDeclarationSyntax { AttributeLists.Count: > 0 },
                transform: static (context, _) => GetSemanticTargetForGeneration(context.SemanticModel, (PropertyDeclarationSyntax)context.Node))
            .Where(static m => m is not null)!;

        IncrementalValueProvider<(Compilation, ImmutableArray<PropertyDeclarationSyntax>)> compilationAndProperties
            = context.CompilationProvider.Combine(propertyDeclarations.Collect());

        context.RegisterSourceOutput(
            compilationAndProperties,
            static (spc, source) => Execute(source.Item1, source.Item2, spc));
    }

    private static PropertyDeclarationSyntax? GetSemanticTargetForGeneration(SemanticModel semanticModel, PropertyDeclarationSyntax propertySyntax)
    {
        var propertySymbol = semanticModel.GetDeclaredSymbol(propertySyntax);

        if (propertySymbol == null)
        {
            return null;
        }

        bool hasAttribute = propertySymbol.GetAttributes().Any(ad =>
            ad.AttributeClass?.ToDisplayString() == Constants.CodebookAttributeFullName);

        if (!hasAttribute)
        {
            return null;
        }

        if (propertySymbol.Type is INamedTypeSymbol propertyType &&
            propertyType.IsGenericType &&
            propertyType.Name == Constants.CodebookRecordTypeName &&
            propertyType.TypeArguments.Length == 1)
        {
            return propertySyntax;
        }

        return null;
    }

    private static void Execute(Compilation compilation, ImmutableArray<PropertyDeclarationSyntax> properties, SourceProductionContext context)
    {
        if (properties.IsDefaultOrEmpty)
        {
            return;
        }

        foreach (var containingClassGroup in properties.GroupBy(p => GetContainingClassSymbol(p, compilation), SymbolEqualityComparer.Default))
        {
            var containingClass = containingClassGroup.Key;
            if (containingClass == null)
            {
                continue;
            }

            if (!IsPartialClass(containingClass))
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    "CBG001",
                    "Generátor",
                    $"Třída '{containingClass.Name}' musí být označena jako 'partial'.",
                    DiagnosticSeverity.Error,
                    DiagnosticSeverity.Error,
                    isEnabledByDefault: true,
                    warningLevel: 0,
                    location: containingClass.Locations.FirstOrDefault()));
                continue;
            }

            var propertiesToGenerate = new List<PropertyToGenerate>();
            foreach (var propertySyntax in containingClassGroup)
            {
                var semanticModel = compilation.GetSemanticModel(propertySyntax.SyntaxTree);
                if (semanticModel.GetDeclaredSymbol(propertySyntax) is IPropertySymbol propertySymbol &&
                    propertySymbol.Type is INamedTypeSymbol propertyType)
                {
                    var codebookTypeArgument = propertyType.TypeArguments[0];
                    string originalPropertyName = propertySymbol.Name;

                    string foreignKeyPropertyName = $"{originalPropertyName}{Constants.CodebookRecordForeignKeyPropertySuffix}";
                    string navigationPropertyName = $"{originalPropertyName}{Constants.CodebookRecordNavigationPropertySuffix}";

                    bool isValueType = codebookTypeArgument.IsValueType;
                    NullableAnnotation nullableAnnotation = propertySymbol.NullableAnnotation;
                    bool isNullable = nullableAnnotation == NullableAnnotation.Annotated;

                    propertiesToGenerate.Add(new PropertyToGenerate(
                        originalPropertyName,
                        foreignKeyPropertyName,
                        navigationPropertyName,
                        codebookTypeArgument.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                        isNullable));
                }
            }

            if (propertiesToGenerate.Count > 0)
            {
                var source = GeneratePartialClassSource(containingClass, propertiesToGenerate);

                context.AddSource($"{containingClass.Name}.{Constants.CodebookInfrastructureFileNameSuffix}", SourceText.From(source, Encoding.UTF8));
            }
        }
    }

    private static INamedTypeSymbol? GetContainingClassSymbol(PropertyDeclarationSyntax propertySyntax, Compilation compilation)
    {
        var semanticModel = compilation.GetSemanticModel(propertySyntax.SyntaxTree);
        return semanticModel.GetDeclaredSymbol(propertySyntax)?.ContainingType;
    }

    private static bool IsPartialClass(ISymbol classSymbol)
    {
        return classSymbol.DeclaringSyntaxReferences
                          .Select(sr => sr.GetSyntax())
                          .OfType<TypeDeclarationSyntax>()
                          .Any(cds => cds.Modifiers.Any(SyntaxKind.PartialKeyword));
    }

    private static string GeneratePartialClassSource(ISymbol containingClass, List<PropertyToGenerate> properties)
    {
        var namespaceName = containingClass.ContainingNamespace.IsGlobalNamespace
            ? null
            : containingClass.ContainingNamespace.ToDisplayString();

        var sb = new StringBuilder();
        sb.AppendLine("// <auto-generated/>");
        sb.AppendLine("#nullable enable");
        sb.AppendLine();

        if (namespaceName != null)
        {
            sb.AppendLine($"namespace {namespaceName};");
            sb.AppendLine();
        }

        sb.AppendLine($"public partial class {containingClass.Name}");
        sb.AppendLine("{");
        sb.AppendLine($"{Constants.PragmaWarningDisable} {Constants.NotUsedWarningCode}");
        sb.AppendLine($"{Constants.PragmaWarningDisable} {Constants.NullableWarningCode}");
        sb.AppendLine();

        foreach (var prop in properties)
        {
            string foreignKeyPropertyNullable = prop.IsNullable ? Constants.Nullable : string.Empty;
            sb.AppendLine($"    {Constants.SummaryStart}");
            sb.AppendLine($"    /// Auto-generated foreign key property for {prop.OriginalPropertyName}");
            sb.AppendLine($"    {Constants.SummaryEnd}");
            sb.AppendLine($"    public string{foreignKeyPropertyNullable} {prop.ForeinKeyPropertyName} {{ get; private set; }}");
            sb.AppendLine();

            string entityTypeFullName = $"{Constants.CodebookRecordEntityNamespace}.{Constants.CodebookRecordEntityTypeName}<{prop.CodebookTypeArgumentFullName}>";

            sb.AppendLine($"    {Constants.SummaryStart}");
            sb.AppendLine($"    /// Auto-generated navigation property for {prop.OriginalPropertyName}");
            sb.AppendLine($"    {Constants.SummaryEnd}");
            sb.AppendLine($"    public {entityTypeFullName}{Constants.Nullable} {prop.NavigationPropertyName} {{ get; set; }}");
            sb.AppendLine();
        }

        sb.AppendLine($"{Constants.PragmaWarningDisable} {Constants.NotUsedWarningCode}");
        sb.AppendLine($"{Constants.PragmaWarningDisable} {Constants.NullableWarningCode}");

        sb.AppendLine("}");

        return sb.ToString();
    }
}