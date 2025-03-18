using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Notifications;
using IncliSafe.Shared.Models.Analysis.Core;

namespace IncliSafe.Shared.Models.Analysis
{
    public class TrendAnalysis : AnalysisBase
    {
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public required decimal TrendValue { get; set; }
        public required decimal Seasonality { get; set; }
        public required decimal Correlation { get; set; }
        public List<TrendData> Data { get; set; } = new();
    }

    public class TrendData
    {
        public required DateTime Timestamp { get; set; }
        public required decimal Value { get; set; }
        public string? Label { get; set; }
        public Dictionary<string, decimal> Metrics { get; set; } = new();
    }
} 