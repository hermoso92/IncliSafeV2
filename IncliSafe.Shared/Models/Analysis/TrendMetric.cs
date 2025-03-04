namespace IncliSafe.Shared.Models.Analysis
{
    public class TrendMetric
    {
        public string Name { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public decimal Change { get; set; }
        public string Direction { get; set; } = string.Empty;
        public string Period { get; set; } = string.Empty;
    }
} 