using Microsoft.VisualStudio.TestTools.UnitTesting;
using OEEMicroservice.Models.OEE;

namespace OEEMicroservice.UnitTest.Models.OEE
{
    [TestClass]
    public class EntityTest
    {
        [TestMethod]
        [DataRow("urn:ngsi-ld:Product:9bc58c8b-bcd7-41cc-b2ce-4b2e59266dfb", "9bc58c8b-bcd7-41cc-b2ce-4b2e59266dfb")]
        public void ProductFullId_InputEntityId_ReturnFullId(string expectedFullId, string id)
        {
            var entity = new Product
            {
                Id = id
            };
            Assert.AreEqual(expectedFullId, entity.GetFullId());
        }

        [TestMethod]
        [DataRow("urn:ngsi-ld:Station:9bc58c8b-bcd7-41cc-b2ce-4b2e59266dfb", "9bc58c8b-bcd7-41cc-b2ce-4b2e59266dfb")]
        public void StationFullId_InputEntityId_ReturnFullId(string expectedFullId, string id)
        {
            var entity = new Station
            {
                Id = id
            };
            Assert.AreEqual(expectedFullId, entity.GetFullId());
        }

        [TestMethod]
        [DataRow("urn:ngsi-ld:OEEMetric:9bc58c8b-bcd7-41cc-b2ce-4b2e59266dfb", "9bc58c8b-bcd7-41cc-b2ce-4b2e59266dfb")]
        public void OEEMetricFullId_InputEntityId_ReturnFullId(string expectedFullId, string id)
        {
            var entity = new OeeMetric
            {
                Id = id
            };
            Assert.AreEqual(expectedFullId, entity.GetFullId());
        }
    }
}
