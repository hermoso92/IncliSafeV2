using System;

namespace IncliSafe.Shared.Models.Analysis
{
    public class HistoricalData
    {
        public DateTime Timestamp { get; set; }
        public decimal Value { get; set; }
        public string MetricType { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public int VehicleId { get; set; }
    }
} 