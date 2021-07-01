using Microsoft.VisualStudio.TestTools.UnitTesting;
using OEEMicroservice.Serializers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OEEMicroservice.UnitTest.Serializers
{
    class NumberEntity
    {
        [JsonPropertyName("number")]
        [JsonConverter(typeof(NumberSerializer))]
        public double Number { get; set; }
    }

    [TestClass]
    public class NumberSerializerTest
    {
        [TestMethod]
        [DataRow(1.4)]
        [DataRow(5.2)]
        public void Serialize_InputIsNumber_ReturnJson(double value)
        {
            var json = "{\"number\":{\"type\":\"" + PropertyType.Number + "\",\"value\":" + value + "}}";

            var result = JsonSerializer.Serialize(new NumberEntity
            {
                Number = value
            });
            
            Assert.AreEqual(result, json);
        }

        [TestMethod]
        [DataRow(1.4)]
        [DataRow(5.2)]
        public void Serialize_InputIsJson_ReturnNumber(double value)
        {
            var json = "{\"number\":" + value + "}";
            var entity = JsonSerializer.Deserialize<NumberEntity>(json);

            Assert.AreEqual(value, entity?.Number);
        }
    }
}
