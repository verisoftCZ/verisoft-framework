using System.Text.Json;
using System.Text.Json.Serialization;
using Verisoft.Core.Common.Entities.Codebooks;

namespace Verisoft.Core.Codebooks.Common.Converters;

public class CodebookRecordJsonConverter<TCodebook> : JsonConverter<CodebookRecord<TCodebook>>
        where TCodebook : CodebookRecordType, new()
{
    public override CodebookRecord<TCodebook>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return default;
        }

        var stringValue = reader.GetString();
        if (stringValue == null)
        {
            return default;
        }

        return stringValue;
    }

    public override void Write(Utf8JsonWriter writer, CodebookRecord<TCodebook> value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.StringValue);
    }
}
