using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Notifications;

namespace IncliSafe.Shared.Models.Analysis
{
    public class DashboardMetrics
    {
        public decimal StabilityScore { get; set; }
        public decimal SafetyScore { get; set; }
        public decimal MaintenanceScore { get; set; }
        public decimal PerformanceScore { get; set; }
        public decimal TrendDirection { get; set; }
        public List<TrendMetric> Trends { get; set; } = new();
        public List<Alert> RecentAlerts { get; set; } = new();
        public List<Anomaly> RecentAnomalies { get; set; } = new();
        public DateTime LastUpdate { get; set; }
    }
} 