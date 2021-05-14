using System.ComponentModel.DataAnnotations;

namespace KPIMicroservice.Models
{
    public class MetricData
    {
        [Required]
        public string ProductName { get; set; }

        [Required]
        public string StationName { get; set; }

        [Required]
        public string BreakDuration { get; set; }

        [Required]
        public string IdealDuration { get; set; }

        public int TotalProductCount { get; set; }
    }
}
