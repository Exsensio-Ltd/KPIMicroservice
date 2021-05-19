using OEEMicroservice.Serializers;
using System;
using System.Text.Json.Serialization;

namespace OEEMicroservice.Models.OEE
{
    public class OeeMetric
    {
        #region Properties

        [JsonPropertyName("id")]
        [JsonConverter(typeof(OeeMetricIdSerializer))]
        public string Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; } = EntityType.OeeMetric;

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

        public string GetFullId() => $"urn:ngsi-ld:{EntityType.OeeMetric}:{Id}";

        #endregion
    }
}
