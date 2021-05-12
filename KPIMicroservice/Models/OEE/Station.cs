using KPIMicroservice.Serializers;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace KPIMicroservice.Models.OEE
{
    public class StationMeta
    {
        [JsonPropertyName("name")]
        [JsonConverter(typeof(TextSerializer))]
        public string Name { get; set; }

        [JsonPropertyName("productionBreakDuration")]
        [JsonConverter(typeof(TextSerializer))]
        public string ProductionBreakDuration { get; set; }

        [JsonPropertyName("productionIdealDuration")]
        [JsonConverter(typeof(TextSerializer))]
        public string ProductionIdealDuration { get; set; }

        [JsonPropertyName("totalProductCount")]
        [JsonConverter(typeof(NumberSerializer))]
        public double TotalProductCount { get; set; }
    }

    public class Station : StationMeta
    {
        #region Properties

        [JsonPropertyName("id")]
        [JsonConverter(typeof(StationIdSerializer))]
        public string Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; } = EntityType.Station;

        [JsonPropertyName("refProduct")]
        [JsonConverter(typeof(RefSerializer))]
        public string RefProduct { get; set; }

        [JsonIgnore]
        public IEnumerable<OEEMetric> Metrics { get; set; }

        #endregion

        #region Methods

        public string GetFullId() => $"urn:ngsi-ld:{EntityType.Station}:{Id}";

        #endregion
    }
}
