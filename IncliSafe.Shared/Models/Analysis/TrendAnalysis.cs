using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis
{
    public class TrendAnalysis : BaseEntity
    {
        public required int VehicleId { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public required decimal TrendValue { get; set; }
        public required decimal Seasonality { get; set; }
        public required decimal Correlation { get; set; }
        public required IncliSafe.Shared.Models.Enums.TrendDirection Direction { get; set; }
        public required IncliSafe.Shared.Models.Enums.PerformanceTrend Performance { get; set; }
        public List<TrendData> Data { get; set; } = new();
        public Dictionary<string, object> Parameters { get; set; } = new();
        public Dictionary<string, object> Metadata { get; set; } = new();
    }

    public class TrendData
    {
        public required DateTime Timestamp { get; set; }
        public required decimal Value { get; set; }
        public string? Label { get; set; }
        public Dictionary<string, decimal> Metrics { get; set; } = new();
    }
} 