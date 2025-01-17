﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using OEEMicroservice.Serializers;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OEEMicroservice.UnitTest.Serializers
{
    class ListEntity
    {
        [JsonPropertyName("listItems")]
        [JsonConverter(typeof(ListSerializer))]
        public List<string> ListItems { get; set; }
    }

    [TestClass]
    public class ListSerializerTest
    {
        [TestMethod]
        public void Serialize_InputIsList_ReturnJson()
        {
            var json = "{\"listItems\":{\"type\":\"" + PropertyType.Text + "\",\"value\":\"1,2,3\"}}";

            var entity = new ListEntity
            {
                ListItems = new List<string> { "1", "2", "3" }
            };
            var result = JsonSerializer.Serialize(entity);
            
            Assert.AreEqual(result, json);
        }

        [TestMethod]
        public void Serialize_InputIsJson_ReturnList()
        {
            var json = "{\"listItems\":\"1,2,3\"}";
            var entity = JsonSerializer.Deserialize<ListEntity>(json);

            Assert.AreEqual(3, entity?.ListItems.Count);
        }
    }
}
