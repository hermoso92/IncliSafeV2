using IncliSafe.Shared.Models.Enums;
namespace IncliSafe.Shared.Models.Analysis
{
    public class TrendMetric
    {
        public required string Name { get; set; } = string.Empty;
        public required decimal Value { get; set; }
        public required decimal Change { get; set; }
        public required string Direction { get; set; } = string.Empty;
        public required string Period { get; set; } = string.Empty;
    }
} 

