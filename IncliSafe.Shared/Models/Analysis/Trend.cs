using System;

namespace IncliSafe.Shared.Models.Analysis
{
    public class Trend
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public string Direction { get; set; } = string.Empty;
        public string Period { get; set; } = string.Empty;
        public decimal ChangeRate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Confidence { get; set; }
    }
} 