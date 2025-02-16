using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Notifications;

namespace IncliSafe.Shared.Models.Analysis
{
    public class DashboardMetrics
    {
        public int TotalVehicles { get; set; }
        public int ActiveAlerts { get; set; }
        public int PendingAnalyses { get; set; }
        public double StabilityIndex { get; set; }
        public double SafetyScore { get; set; }
        public double MaintenanceScore { get; set; }
        public List<TrendMetric> Trends { get; set; } = new();
        public List<Alert> RecentAlerts { get; set; } = new();
        public List<Anomaly> RecentAnomalies { get; set; } = new();
        public DateTime LastUpdate { get; set; }
        public Dictionary<string, double> PatternDistribution { get; set; } = new();
    }
} 