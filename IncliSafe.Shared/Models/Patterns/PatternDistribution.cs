namespace IncliSafe.Shared.Models.Patterns
{
    public class PatternDistribution
    {
        public int Id { get; set; }
        public string PatternName { get; set; } = string.Empty;
        public decimal ConfidenceScore { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime DetectionTime { get; set; }
        public PatternType Type { get; set; }
        public string Category { get; set; } = string.Empty;
        public double Percentage { get; set; }
        public int Count { get; set; }
        public List<string> Examples { get; set; } = new();
    }
} 