using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KPIMicroservice.Serializers
{
    public class DateTimeSerializer : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("type", PropertyType.DateTimeType);
            writer.WriteString("value", value.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            writer.WriteEndObject();
        }
    }
}
