using System;

namespace IncliSafe.Shared.Models.Analysis
{
    public class Seasonality
    {
        public int Id { get; set; }
        public string Pattern { get; set; } = string.Empty;
        public decimal Amplitude { get; set; }
        public decimal Period { get; set; }
        public decimal Confidence { get; set; }
        public string MetricType { get; set; } = string.Empty;
    }
} 