namespace OEEMicroservice.DTOs
{
    public class DataSet
    {
        /// <summary>
        /// Text label as date time
        /// </summary>
        /// <example>12:00 29-03</example>
        public string Text { get; set; }
        
        /// <summary>
        /// OEE value
        /// </summary>
        /// <example>89</example>
        public int Value { get; set; }
        
        /// <summary>
        /// Availability takes into account all events that stop planned production long enough where it makes sense
        /// to track a reason for being down (typically several minutes).
        /// </summary>
        /// <example>98</example>
        public double Availability { get; set; } = 0;
        
        /// <summary>
        /// Performance OEE Performance – One of the three OEE Factors.
        /// Takes into account Performance Loss (factors that cause the process to operate at less than the maximum possible speed, when running).
        /// Must be measured in an OEE program, usually by comparing Actual Cycle Time (or Actual Run Rate) to Ideal Cycle Time (or Ideal Run Rate).
        /// takes into account anything that causes the manufacturing process to run at less than the maximum possible speed when it is running
        /// </summary>
        /// <example>76</example>
        public double Performance { get; set; } = 0;
        
        /// <summary>
        /// Quality takes into account manufactured parts that do not meet quality standards, including parts that need rework.
        /// </summary>
        /// <example>85</example>
        public double Quality { get; set; } = 0;
    }
}
