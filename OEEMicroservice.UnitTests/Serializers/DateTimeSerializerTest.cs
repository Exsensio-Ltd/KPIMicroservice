﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using OEEMicroservice.Serializers;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OEEMicroservice.UnitTest.Serializers
{
    class DateTimeEntity
    {
        [JsonPropertyName("createdTime")]
        [JsonConverter(typeof(DateTimeSerializer))]
        public DateTime CreatedTime { get; set; }
    }

    [TestClass]
    public class DateTimeSerializerTest
    {
        [TestMethod]
        public void Serialize_InputIsDateTime_ReturnJson()
        {
            var createdTime = DateTime.Now;
            var json = "{\"createdTime\":{\"type\":\"" + PropertyType.DateTimeType + "\",\"value\":\"" + createdTime.ToString("yyyy-MM-ddTHH:mm:ssZ") + "\"}}";

            var result = JsonSerializer.Serialize(new DateTimeEntity
            {
                CreatedTime = createdTime
            });
            
            Assert.AreEqual(result, json);
        }

        [TestMethod]
        public void Serialize_InputIsJson_ReturnDateTime()
        {
            var createdTime = DateTime.Now;
            var json = "{\"createdTime\":\"" + createdTime.ToString("yyyy-MM-ddTHH:mm:ss") + "\"}";

            var entity = JsonSerializer.Deserialize<DateTimeEntity>(json);

            Assert.AreEqual(createdTime.ToString("yyyy-MM-ddTHH:mm:ss"), entity?.CreatedTime.ToString("yyyy-MM-ddTHH:mm:ss"));
        }
    }
}
