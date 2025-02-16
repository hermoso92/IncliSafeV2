using System;

namespace IncliSafe.Shared.Models.Analysis
{
    public class Trend
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Slope { get; set; }
        public decimal Intercept { get; set; }
        public decimal R2 { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string MetricType { get; set; } = string.Empty;
    }
} 