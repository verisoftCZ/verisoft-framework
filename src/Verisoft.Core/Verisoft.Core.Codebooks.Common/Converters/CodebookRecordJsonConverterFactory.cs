using System.Text.Json;
using System.Text.Json.Serialization;
using Verisoft.Core.Common.Entities.Codebooks;

namespace Verisoft.Core.Codebooks.Common.Converters;

public class CodebookRecordJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(CodebookRecord<>);
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var codebookType = typeToConvert.GetGenericArguments()[0];

        var converterType = typeof(CodebookRecordJsonConverter<>).MakeGenericType(codebookType);

        return Activator.CreateInstance(converterType) as JsonConverter;
    }
}
