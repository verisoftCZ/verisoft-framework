using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Verisoft.Core.AspNet.Filters;

public class NullableObjectSchemaFilter(string namespaceStart) : ISchemaFilter
{
    private const string ObjectType = "object";

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema.Type == ObjectType && context.Type.FullName.StartsWith(namespaceStart))
        {
            schema.Nullable = true;
        }
    }
}
