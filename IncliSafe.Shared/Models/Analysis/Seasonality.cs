using IncliSafe.Shared.Models.Enums;
using System;

namespace IncliSafe.Shared.Models.Analysis
{
    public class Seasonality
    {
        public required int Id { get; set; }
        public required string Pattern { get; set; } = string.Empty;
        public required decimal Amplitude { get; set; }
        public required decimal Period { get; set; }
        public required decimal Confidence { get; set; }
        public required string MetricType { get; set; } = string.Empty;
    }
} 

