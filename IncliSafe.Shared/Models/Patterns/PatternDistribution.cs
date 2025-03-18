using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Patterns
{
    public class PatternDistribution
    {
        public required int Id { get; set; }
        public required string PatternName { get; set; } = string.Empty;
        public required decimal ConfidenceScore { get; set; }
        public required string Description { get; set; } = string.Empty;
        public required DateTime DetectionTime { get; set; }
        public required PatternType Type { get; set; }
        public required string Category { get; set; } = string.Empty;
        public required double Percentage { get; set; }
        public required int Count { get; set; }
        public List<string> Examples { get; set; } = new();
    }
} 
