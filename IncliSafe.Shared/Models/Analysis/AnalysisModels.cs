using IncliSafe.Shared.Models.Enums;
using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Notifications;
using IncliSafe.Shared.Models.Analysis.Core;

namespace IncliSafe.Shared.Models.Analysis
{
    // Modelos base
    public class AnalysisBase : BaseEntity
    {
        public required int VehicleId { get; set; }
        public required DateTime AnalysisDate { get; set; }
        public required AnalysisType Type { get; set; }
        public required decimal StabilityScore { get; set; }
        public required decimal SafetyScore { get; set; }
        public required decimal MaintenanceScore { get; set; }
        public string? Notes { get; set; }
        public Vehicle? Vehicle { get; set; }
    }

    public class DobackAnalysis : AnalysisBase
    {
        public required int DobackId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal StabilityIndex { get; set; }
        public required decimal SafetyIndex { get; set; }
        public required decimal MaintenanceIndex { get; set; }
        public List<DobackData> Data { get; set; } = new();
        public List<Anomaly> Anomalies { get; set; } = new();
        public List<AnalysisPrediction> Predictions { get; set; } = new();
        public List<DetectedPattern> Patterns { get; set; } = new();
    }

    public class TrendAnalysis : AnalysisBase
    {
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public required decimal TrendValue { get; set; }
        public required decimal Seasonality { get; set; }
        public required decimal Correlation { get; set; }
        public List<TrendData> Data { get; set; } = new();
    }

    public class DobackData
    {
        public required DateTime Timestamp { get; set; }
        public required decimal Value { get; set; }
        public required int SensorId { get; set; }
        public string? MetricType { get; set; }
        public Dictionary<string, decimal> AdditionalMetrics { get; set; } = new();
    }

    public class TrendData
    {
        public required DateTime Timestamp { get; set; }
        public required decimal Value { get; set; }
        public string? Label { get; set; }
        public Dictionary<string, decimal> Metrics { get; set; } = new();
    }

    public class DetectedPattern
    {
        public required string Type { get; set; }
        public string? Description { get; set; }
        public required decimal Confidence { get; set; }
        public List<PatternDataPoint> DataPoints { get; set; } = new();
        public Dictionary<string, string> Metadata { get; set; } = new();
    }

    public class PatternDataPoint
    {
        public required DateTime Timestamp { get; set; }
        public string? Label { get; set; }
        public required decimal Value { get; set; }
        public Dictionary<string, decimal> Metrics { get; set; } = new();
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

    public class CoreMetrics
    {
        public required decimal StabilityScore { get; set; }
        public required decimal SafetyScore { get; set; }
        public required decimal MaintenanceScore { get; set; }
        public required decimal OverallHealth { get; set; }
        public required DateTime LastUpdated { get; set; }
        public Dictionary<string, decimal> CustomMetrics { get; set; } = new();
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

    public class AnalysisResult : BaseEntity
    {
        public new int Id { get; set; }
        public new DateTime CreatedAt { get; set; }
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

    public class PredictionResult
    {
        public required int Id { get; set; }
        public required int VehicleId { get; set; }
        public required DateTime PredictedAt { get; set; }
        public required PredictionType Type { get; set; }
        public required decimal Probability { get; set; }
        public required decimal PredictedValue { get; set; }
        public required string Description { get; set; } = string.Empty;
        public List<string> Recommendations { get; set; } = new();
        public virtual Vehicle Vehicle { get; set; } = null!;
    }

    public class AnalysisPattern : BaseEntity
    {
        public new int Id { get; set; }
        public new DateTime CreatedAt { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required PatternType Type { get; set; }
        public required decimal Confidence { get; set; }
    }

    public class AnalysisAlert : BaseEntity
    {
        public new int Id { get; set; }
        public new DateTime CreatedAt { get; set; }
        public required string Title { get; set; }
        public required string Message { get; set; }
        public required AlertSeverity Severity { get; set; }
        public required DateTime Timestamp { get; set; }
        public required bool IsAcknowledged { get; set; }
        public required string AcknowledgedBy { get; set; }
        public DateTime? AcknowledgedAt { get; set; }
        public List<SensorReading> RelatedReadings { get; set; } = new();
    }

    public class AnalysisPatternDetails : BaseEntity
    {
        public new int Id { get; set; }
        public new DateTime CreatedAt { get; set; }
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

    public class SensorReading : BaseEntity
    {
        public new int Id { get; set; }
        public new DateTime CreatedAt { get; set; }
        public required string SensorId { get; set; }
        public required string Type { get; set; }
        public required double Value { get; set; }
        public required DateTime Timestamp { get; set; }
        public Dictionary<string, string> Metadata { get; set; } = new();
    }

    public class AnalysisConfiguration
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string AnalysisType { get; set; }
    }
} 





