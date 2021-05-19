namespace KPIMicroservice.Models
{
    public class MetaData
    {
        /// <summary>
        /// Unproductive time where the process is scheduled not to run because the crew is scheduled to be away from the line
        /// </summary>
        /// <example>00:00:01.100</example>
        public string ProductionBreakDuration { get; set; }

        /// <summary>
        /// Theoretical minimum time to produce one part
        /// </summary>
        /// <example>00:00:55.123</example>
        public string ProductionIdealDuration { get; set; }

        /// <summary>
        /// Total of all produced parts
        /// </summary>
        /// <example>1</example>
        public double TotalProductCount { get; set; }
    }
}
