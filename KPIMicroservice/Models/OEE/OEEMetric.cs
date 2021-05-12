using KPIMicroservice.Serializers;
using System;
using System.Text.Json.Serialization;

namespace KPIMicroservice.Models.OEE
{
    public class OEEMetric
    {
        #region Properties

        [JsonPropertyName("id")]
        [JsonConverter(typeof(OEEMetricIdSerializer))]
        public string Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; } = EntityType.OEEMetric;

        [JsonPropertyName("createdTime")]
        [JsonConverter(typeof(DateTimeSerializer))]
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("goodProductCount")]
        [JsonConverter(typeof(IntegerSerializer))]
        public int GoodProductCount { get; set; } = 1;

        [JsonPropertyName("refStation")]
        [JsonConverter(typeof(RefSerializer))]
        public string RefStation { get; set; }

        [JsonIgnore]
        public TimeSpan ProductionShiftDuration { get; set; }

        #endregion

        #region Methods

        public string GetFullId() => $"urn:ngsi-ld:{EntityType.OEEMetric}:{Id}";

        #endregion
    }
}
