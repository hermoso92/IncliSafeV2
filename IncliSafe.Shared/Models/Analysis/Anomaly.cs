using System;
using System.Collections.Generic;

namespace IncliSafe.Shared.Models.Analysis
{
    public class Anomaly
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public DateTime DetectedAt { get; set; }
        public string Description { get; set; } = string.Empty;
        public double ExpectedValue { get; set; }
        public double ActualValue { get; set; }
        public double Deviation { get; set; }
        public string Severity { get; set; } = string.Empty;
        public AnomalyType Type { get; set; }
        public List<string> PossibleCauses { get; set; } = new();
        public List<string> RecommendedActions { get; set; } = new();
        public virtual Vehiculo Vehicle { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        public double Value { get; set; }
        public NotificationSeverity NotificationSeverity { get; set; }
        public int? TrendAnalysisId { get; set; }
        public virtual TrendAnalysis? TrendAnalysis { get; set; }
    }

    public enum AnomalyType
    {
        Stability,
        Safety,
        Performance,
        Maintenance,
        Pattern,
        Prediction,
        System,
        Acceleration,
        Orientation,
        Speed,
        High,
        Low,
        Seasonal,
        Trend
    }
}