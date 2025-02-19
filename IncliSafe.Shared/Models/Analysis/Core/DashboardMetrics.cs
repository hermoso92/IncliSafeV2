namespace IncliSafe.Shared.Models.Analysis.Core
{
    public class DashboardMetrics
    {
        public decimal StabilityScore { get; set; }
        public decimal SafetyScore { get; set; }
        public TrendMetrics Trends { get; set; } = new();
    }

    public class TrendMetrics
    {
        public TrendData ShortTerm { get; set; } = new();
        public TrendData MediumTerm { get; set; } = new();
        public TrendData LongTerm { get; set; } = new();
    }

    public class TrendData
    {
        public decimal Value { get; set; }
        public string Direction { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
} 