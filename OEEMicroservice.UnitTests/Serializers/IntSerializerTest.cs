using Microsoft.VisualStudio.TestTools.UnitTesting;
using OEEMicroservice.Serializers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OEEMicroservice.UnitTest.Serializers
{
    class IntEntity
    {
        [JsonPropertyName("number")]
        [JsonConverter(typeof(IntegerSerializer))]
        public int Number { get; set; }
    }

    [TestClass]
    public class IntSerializerTest
    {
        [TestMethod]
        [DataRow(1)]
        [DataRow(5)]
        public void Serialize_InputIsInt_ReturnJson(int value)
        {
            var json = "{\"number\":{\"type\":\"" + PropertyType.Integer + "\",\"value\":" + value + "}}";

            var result = JsonSerializer.Serialize(new IntEntity
            {
                Number = value
            });
            
            Assert.AreEqual(result, json);
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(5)]
        public void Serialize_InputIsJson_ReturnInt(int value)
        {
            var json = "{\"number\":" + value + "}";
            var entity = JsonSerializer.Deserialize<IntEntity>(json);

            Assert.AreEqual(value, entity?.Number);
        }
    }
}
