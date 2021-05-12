namespace KPIMicroservice.Models
{
    public class DataSet
    {
        public string Text { get; set; }
        public int Value { get; set; }
        public double Availability { get; set; } = 0;
        public double Performance { get; set; } = 0;
        public double Quality { get; set; } = 0;
    }
}
