using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KPIMicroservice.Serializers
{
    public class IntegerSerializer : JsonConverter<int>
    {
        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return 0;
            }
            return reader.GetInt32();
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("type", PropertyType.Integer);
            writer.WriteNumber("value", value);
            writer.WriteEndObject();
        }
    }
}
