using Microsoft.VisualStudio.TestTools.UnitTesting;
using OEEMicroservice.Serializers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OEEMicroservice.UnitTest.Serializers
{
    class RefEntity
    {
        [JsonPropertyName("ref")]
        [JsonConverter(typeof(RefSerializer))]
        public string Ref { get; set; }
    }

    [TestClass]
    public class RefSerializerTest
    {
        [TestMethod]
        [DataRow("9bc58c8b-bcd7-41cc-b2ce-4b2e59266dfb")]
        public void Serialize_InputIsId_ReturnJson(string value)
        {
            var json = "{\"ref\":{\"type\":\"" + PropertyType.Relationship + "\",\"value\":\"" + value + "\"}}";

            var result = JsonSerializer.Serialize(new RefEntity
            {
                Ref = value
            });
            
            Assert.AreEqual(result, json);
        }

        [TestMethod]
        [DataRow("9bc58c8b-bcd7-41cc-b2ce-4b2e59266dfb")]
        public void Serialize_InputIsJson_ReturnId(string value)
        {
            var json = "{\"ref\":\"" + value + "\"}";
            var entity = JsonSerializer.Deserialize<RefEntity>(json);

            Assert.AreEqual(value, entity?.Ref);
        }
    }
}
