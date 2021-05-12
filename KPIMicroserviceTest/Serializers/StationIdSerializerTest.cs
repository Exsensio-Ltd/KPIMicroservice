using Microsoft.VisualStudio.TestTools.UnitTesting;
using KPIMicroservice.Serializers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KPIMicroserviceTest.Serializers
{
    class StationIdEntity
    {
        [JsonPropertyName("id")]
        [JsonConverter(typeof(StationIdSerializer))]
        public string Id { get; set; }
    }

    [TestClass]
    public class StationIdSerializerTest_Serialize
    {
        [TestMethod]
        [DataRow("9bc58c8b-bcd7-41cc-b2ce-4b2e59266dfb", EntityType.Station)]
        public void Serialize_InputIsId_ReturnJson(string value, string entityType)
        {
            var json = "{\"id\":\"urn:ngsi-ld:" + entityType + ":" + value + "\"}";

            var result = JsonSerializer.Serialize(new StationIdEntity
            {
                Id = value
            });

            Assert.AreEqual(result, json);
        }

        [TestMethod]
        [DataRow("9bc58c8b-bcd7-41cc-b2ce-4b2e59266dfb", EntityType.Station)]
        public void Serialize_InputIsJson_ReturnId(string value, string entityType)
        {
            var json = "{\"id\":\"urn:ngsi-ld:" + entityType + ":" + value + "\"}";
            var entity = JsonSerializer.Deserialize<StationIdEntity>(json);

            Assert.AreEqual(value, entity.Id);
        }
    }
}
