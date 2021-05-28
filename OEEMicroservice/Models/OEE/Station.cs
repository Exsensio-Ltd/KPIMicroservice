using OEEMicroservice.Serializers;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OEEMicroservice.Models.OEE
{
    public class StationMeta
    {
        #region Properties

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
        public double TotalProductCount { get; set; } = 0;

        #endregion

        #region Methods

        public bool IsUpdated(Station station)
        {
            if (station == null)
            {
                return false;
            }

            return !string.IsNullOrEmpty(station.ProductionBreakDuration) &&
                   ProductionBreakDuration != station.ProductionBreakDuration ||
                   !string.IsNullOrEmpty(station.ProductionIdealDuration) &&
                   ProductionIdealDuration != station.ProductionIdealDuration ||
                   !TotalProductCount.Equals(station.TotalProductCount);
        }

        #endregion
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
        public IEnumerable<OeeMetric> Metrics { get; set; }

        #endregion

        #region Methods

        public string GetFullId() => $"urn:ngsi-ld:{EntityType.Station}:{Id}";

        #endregion
    }
}
