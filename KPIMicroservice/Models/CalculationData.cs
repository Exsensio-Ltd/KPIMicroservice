using KPIMicroservice.Utils.Calculator;
using System.ComponentModel.DataAnnotations;

namespace KPIMicroservice.Models
{
    public class CalculationData
    {
        /// <summary>
        /// Station entity Id
        /// </summary>
        /// <example>urn:ngsi-ld:Station:8b960a8e-ab44-40e6-aaed-8499cb428d18</example>
        [Required]
        public string Id { get; set; }
        
        /// <summary>
        /// Reporting period in hours
        /// </summary>
        /// <example>1</example>
        [Required]
        public ReportingPeriod ReportingPeriod { get; set; }
        
        /// <summary>
        /// OEE calculation type
        /// </summary>
        /// <example>1</example>
        [Required]
        public CalculationType Type { get; set; }
    }
}
