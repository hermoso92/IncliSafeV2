using IncliSafe.Shared.Models.Enums;
using System;

namespace IncliSafe.Shared.Models.Analysis
{
    public class Trend
    {
        public required int Id { get; set; }
        public required string Name { get; set; } = string.Empty;
        public required decimal Value { get; set; }
        public required string Direction { get; set; } = string.Empty;
        public required string Period { get; set; } = string.Empty;
        public required decimal ChangeRate { get; set; }
        public required string Status { get; set; } = string.Empty;
        public required string Description { get; set; } = string.Empty;
        public required decimal Confidence { get; set; }
    }
} 

