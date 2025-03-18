using IncliSafe.Shared.Models.Enums;
namespace IncliSafe.Shared.Models.Analysis
{
    public class Metric
    {
        public required int Id { get; set; }
        public required string Name { get; set; } = string.Empty;
        public required decimal Value { get; set; }
        public required string Unit { get; set; } = string.Empty;
        public required string Category { get; set; } = string.Empty;
        public required decimal MinValue { get; set; }
        public required decimal MaxValue { get; set; }
        public required decimal WarningThreshold { get; set; }
        public required decimal CriticalThreshold { get; set; }
        public required string Status { get; set; } = string.Empty;
        public required string Description { get; set; } = string.Empty;
    }
} 

