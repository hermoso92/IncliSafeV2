using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafe.Shared.Models.Analysis.Core
{
    public class AnalysisPrediction
    {
        public required int Id { get; set; }
        public required int VehicleId { get; set; }
        public required DateTime PredictedAt { get; set; }
        public required PredictionType Type { get; set; }
        public required decimal Probability { get; set; }
        public required string Description { get; set; } = string.Empty;
        public required decimal PredictedValue { get; set; }
        public int? AnalysisId { get; set; }
        public virtual Vehicle Vehicle { get; set; } = null!;
        public virtual DobackAnalysis? Analysis { get; set; }
    }

    public class AnalysisResult
    {
        public required int Id { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string AnalysisType { get; set; }
        public required decimal StabilityScore { get; set; }
        public required decimal SafetyScore { get; set; }
        public required decimal EfficiencyScore { get; set; }
        public required decimal MaintenanceScore { get; set; }
        public required DateTime AnalysisTime { get; set; }
        public required int VehicleId { get; set; }
        public required int DobackAnalysisId { get; set; }
        public List<string> Recommendations { get; set; } = new();
        public virtual DobackAnalysis DobackAnalysis { get; set; } = null!;
    }

    public class Anomaly
    {
        public required int Id { get; set; }
        public required int VehicleId { get; set; }
        public required DateTime DetectedAt { get; set; }
        public required AnomalyType Type { get; set; }
        public required AlertSeverity Severity { get; set; }
        public required string Description { get; set; } = string.Empty;
        public required decimal ExpectedValue { get; set; }
        public required decimal ActualValue { get; set; }
        public required decimal Deviation { get; set; }
        public int? AnalysisId { get; set; }
        public virtual Vehicle Vehicle { get; set; } = null!;
        public virtual DobackAnalysis? Analysis { get; set; }
    }

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

    public class PredictionTypeInfo
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required PredictionType Type { get; set; }
        public required decimal Confidence { get; set; }
    }

    public class PatternType
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required PatternType Type { get; set; }
        public required decimal Confidence { get; set; }
    }

    public class PatternDetails
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required PatternType Type { get; set; }
        public required decimal Confidence { get; set; }
        public required DateTime DetectedAt { get; set; }
        public required bool IsActive { get; set; }
        public List<PatternDataPoint> DataPoints { get; set; } = new();
        public Dictionary<string, string> Parameters { get; set; } = new();
        public Dictionary<string, string> Metadata { get; set; } = new();
    }
} 



