using OEEMicroservice.Serializers;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OEEMicroservice.Models.OEE
{
    public class Product
    {
        #region Properties

        [JsonPropertyName("id")]
        [JsonConverter(typeof(ProductIdSerializer))]
        public string Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; } = EntityType.Product;

        [JsonPropertyName("name")]
        [JsonConverter(typeof(TextSerializer))]
        public string Name { get; set; }

        [JsonIgnore]
        public IEnumerable<Station> Stations { get; set; }

        #endregion

        #region Methods

        public string GetFullId() => $"urn:ngsi-ld:{EntityType.Product}:{Id}";

        #endregion
    }
}
