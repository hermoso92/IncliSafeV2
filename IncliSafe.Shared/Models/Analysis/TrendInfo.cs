using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;

namespace IncliSafe.Shared.Models.Analysis
{
    public class TrendInfo
    {
        public required int Id { get; set; }
        public required DateTime Timestamp { get; set; }
        public required string MetricType { get; set; }
        public required decimal Value { get; set; }
        public required decimal PreviousValue { get; set; }
        public required decimal Change { get; set; }
        public required decimal ChangePercentage { get; set; }
        public required TrendDirection Direction { get; set; }
        public required PerformanceTrend Performance { get; set; }
        public List<decimal> HistoricalValues { get; set; } = new();
        public List<DateTime> HistoricalDates { get; set; } = new();
    }
} 

