using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Verisoft.Core.AspNet.Filters;

public class ExcludeDeclaredTypePropertySchemaFilter : ISchemaFilter
{
    private const string PropertyNameToRemove = "DeclaredType";

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (typeof(ObjectResult).IsAssignableFrom(context.Type))
        {
            var propertyKey = schema.Properties.Keys.FirstOrDefault(k => string.Equals(k, PropertyNameToRemove, StringComparison.OrdinalIgnoreCase));
            if (propertyKey != null)
            {
                schema.Properties.Remove(propertyKey);
            }
        }
    }
}
