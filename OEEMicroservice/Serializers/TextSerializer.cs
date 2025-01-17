﻿using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OEEMicroservice.Serializers
{
    public class TextSerializer : JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetString();
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("type", PropertyType.Text);
            writer.WriteString("value", value);
            writer.WriteEndObject();
        }
    }
}
