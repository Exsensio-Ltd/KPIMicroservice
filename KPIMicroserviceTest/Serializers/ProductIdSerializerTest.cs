using Microsoft.VisualStudio.TestTools.UnitTesting;
using KPIMicroservice.Serializers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KPIMicroserviceTest.Serializers
{
    class ProductIdEntity
    {
        [JsonPropertyName("id")]
        [JsonConverter(typeof(ProductIdSerializer))]
        public string Id { get; set; }
    }

    [TestClass]
    public class ProductIdSerializerTest_Serialize
    {
        [TestMethod]
        [DataRow("9bc58c8b-bcd7-41cc-b2ce-4b2e59266dfb", EntityType.Product)]
        public void Serialize_InputIsId_ReturnJson(string value, string entityType)
        {
            var json = "{\"id\":\"urn:ngsi-ld:" + entityType + ":" + value + "\"}";

            var result = JsonSerializer.Serialize(new ProductIdEntity
            {
                Id = value
            });

            Assert.AreEqual(result, json);
        }

        [TestMethod]
        [DataRow("9bc58c8b-bcd7-41cc-b2ce-4b2e59266dfb", EntityType.Product)]
        public void Serialize_InputIsJson_ReturnId(string value, string entityType)
        {
            var json = "{\"id\":\"urn:ngsi-ld:" + entityType + ":" + value + "\"}";
            var entity = JsonSerializer.Deserialize<ProductIdEntity>(json);

            Assert.AreEqual(value, entity.Id);
        }
    }
}
