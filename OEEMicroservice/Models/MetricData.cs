using System.ComponentModel.DataAnnotations;

namespace OEEMicroservice.Models
{
    public class MetricData
    {
        /// <example>Test Product</example>>
        [Required]
        public string ProductName { get; set; }

        /// <example>Test Station</example>>
        [Required]
        public string StationName { get; set; }

        /// <summary>
        /// Unproductive time where the process is scheduled not to run because the crew is scheduled to be away from the line
        /// </summary>
        /// <example>00:00:01.221</example>
        [Required]
        public string ProductionBreakDuration { get; set; }

        /// <summary>
        /// Theoretical minimum time to produce one part
        /// </summary>
        /// <example>00:00:55.123</example>
        [Required]
        public string ProductionIdealDuration { get; set; }

        /// <summary>
        /// Total of all produced parts
        /// </summary>
        /// <example>1</example>
        public int TotalProductCount { get; set; }
    }
}
