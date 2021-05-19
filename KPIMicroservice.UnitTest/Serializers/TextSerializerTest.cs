using Microsoft.VisualStudio.TestTools.UnitTesting;
using KPIMicroservice.Serializers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KPIMicroservice.UnitTest.Serializers
{
    class TextEntity
    {
        [JsonPropertyName("text")]
        [JsonConverter(typeof(TextSerializer))]
        public string Text { get; set; }
    }

    [TestClass]
    public class TextSerializerTest
    {
        [TestMethod]
        [DataRow("test")]
        public void Serialize_InputIsText_ReturnJson(string value)
        {
            var json = "{\"text\":{\"type\":\"" + PropertyType.Text + "\",\"value\":\"" + value + "\"}}";

            var result = JsonSerializer.Serialize(new TextEntity
            {
                Text = value
            });
            
            Assert.AreEqual(result, json);
        }

        [TestMethod]
        [DataRow("test")]
        public void Serialize_InputIsJson_ReturnText(string value)
        {
            var json = "{\"text\":\"" + value + "\"}";
            var entity = JsonSerializer.Deserialize<TextEntity>(json);

            Assert.AreEqual(value, entity?.Text);
        }
    }
}
