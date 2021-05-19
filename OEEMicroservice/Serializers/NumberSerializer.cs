using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OEEMicroservice.Serializers
{
    public class NumberSerializer : JsonConverter<double>
    {
        public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType == JsonTokenType.Null ? 0 : reader.GetDouble();
        }

        public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("type", PropertyType.Number);
            writer.WriteNumber("value", value);
            writer.WriteEndObject();
        }
    }
}
