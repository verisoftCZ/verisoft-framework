using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Verisoft.Core.Common.Entities.Codebooks;

namespace Verisoft.Core.Codebooks.Common.Extensions;

public static class CodebookSwaggerExtensions
{
    public static void MapCodebookRecordsFromAssemblies(
        this SwaggerGenOptions options,
        params Assembly[] assemblies)
    {
        var baseType = typeof(CodebookRecordType);

        var codebookTypes = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract)
            .Where(baseType.IsAssignableFrom)
            .ToList();

        foreach (var t in codebookTypes)
        {
            var recordType = typeof(CodebookRecord<>).MakeGenericType(t);

            options.MapType(recordType, () => new OpenApiSchema { Type = "string" });
        }
    }
}
