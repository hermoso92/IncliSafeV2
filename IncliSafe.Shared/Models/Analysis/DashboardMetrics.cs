using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Notifications;
using IncliSafe.Shared.Models.Analysis.Core;

namespace IncliSafe.Shared.Models.Analysis
{
    public class DashboardMetrics
    {
        public required decimal StabilityScore { get; set; }
        public required decimal SafetyScore { get; set; }
        public required decimal MaintenanceScore { get; set; }
        public required int TotalAnalyses { get; set; }
        public required DateTime LastAnalysisTime { get; set; }
        public List<Alert> RecentAlerts { get; set; } = new();
        public List<Anomaly> RecentAnomalies { get; set; } = new();
        public required TrendMetrics Trends { get; set; } = new();
    }

    public class TrendMetrics
    {
        public required decimal StabilityTrend { get; set; }
        public required decimal SafetyTrend { get; set; }
        public required decimal MaintenanceTrend { get; set; }
        public required TrendDirection StabilityDirection { get; set; }
        public required TrendDirection SafetyDirection { get; set; }
        public required TrendDirection MaintenanceDirection { get; set; }
        public required PerformanceTrend OverallPerformance { get; set; }
    }
} 