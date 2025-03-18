using IncliSafe.Shared.Models.Enums;
using System;

namespace IncliSafe.Shared.Models.Analysis
{
    public class HistoricalData
    {
        public required DateTime Timestamp { get; set; }
        public required decimal Value { get; set; }
        public required string MetricType { get; set; } = string.Empty;
        public required string Source { get; set; } = string.Empty;
        public required int VehicleId { get; set; }
    }
} 

