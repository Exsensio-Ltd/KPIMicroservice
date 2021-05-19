using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KPIMicroservice.Serializers
{
    public class ProductIdSerializer : JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetString()?.Split(":").Last();
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStringValue($"urn:ngsi-ld:{EntityType.Product}:{value}");
        }
    }
}
