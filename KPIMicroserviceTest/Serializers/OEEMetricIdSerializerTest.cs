﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using KPIMicroservice.Serializers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KPIMicroserviceTest.Serializers
{
    class OEEMetricIdEntity
    {
        [JsonPropertyName("id")]
        [JsonConverter(typeof(OEEMetricIdSerializer))]
        public string Id { get; set; }
    }

    [TestClass]
    public class OEEMetricIdSerializerTest_Serialize
    {
        [TestMethod]
        [DataRow("9bc58c8b-bcd7-41cc-b2ce-4b2e59266dfb", EntityType.OEEMetric)]
        public void Serialize_InputIsId_ReturnJson(string value, string entityType)
        {
            var json = "{\"id\":\"urn:ngsi-ld:" + entityType + ":" + value + "\"}";

            var result = JsonSerializer.Serialize(new OEEMetricIdEntity
            {
                Id = value
            });
            
            Assert.AreEqual(result, json);
        }

        [TestMethod]
        [DataRow("9bc58c8b-bcd7-41cc-b2ce-4b2e59266dfb", EntityType.OEEMetric)]
        public void Serialize_InputIsJson_ReturnId(string value, string entityType)
        {
            var json = "{\"id\":\"urn:ngsi-ld:" + entityType + ":" + value + "\"}";
            var entity = JsonSerializer.Deserialize<OEEMetricIdEntity>(json);

            Assert.AreEqual(value, entity.Id);
        }
    }
}