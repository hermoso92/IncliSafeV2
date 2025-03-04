using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Analysis;

namespace IncliSafe.Shared.Models.Analysis
{
    // Modelos base
    public abstract class AnalysisBase
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public DateTime AnalysisDate { get; set; }
        public AnalysisType Type { get; set; }
        public decimal StabilityScore { get; set; }
        public decimal SafetyScore { get; set; }
        public decimal MaintenanceScore { get; set; }
        public string Notes { get; set; } = string.Empty;
        public virtual Vehiculo Vehicle { get; set; } = null!;
    }

    public class DobackAnalysis : AnalysisBase
    {
        public string DobackId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal StabilityIndex { get; set; }
        public decimal SafetyIndex { get; set; }
        public decimal MaintenanceIndex { get; set; }
        public List<DobackData> Data { get; set; } = new();
        public List<Anomaly> Anomalies { get; set; } = new();
        public List<AnalysisPrediction> Predictions { get; set; } = new();
        public List<PatternDetails> Patterns { get; set; } = new();
        public override Vehiculo Vehicle { get; set; } = null!;
    }

    public class TrendAnalysis : AnalysisBase
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TrendValue { get; set; }
        public decimal Seasonality { get; set; }
        public decimal Correlation { get; set; }
        public List<TrendData> Data { get; set; } = new();
        public override Vehiculo Vehicle { get; set; } = null!;
    }

    public class TrendData
    {
        public DateTime Timestamp { get; set; }
        public decimal Value { get; set; }
        public string Label { get; set; } = string.Empty;
    }

    public class PatternDetails
    {
        public int Id { get; set; }
        public string PatternId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Confidence { get; set; }
        public List<TrendData> Data { get; set; } = new();
        public int DobackAnalysisId { get; set; }
        public virtual DobackAnalysis DobackAnalysis { get; set; } = null!;
    }

    public class Anomaly
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public DateTime DetectedAt { get; set; }
        public AnomalyType Type { get; set; }
        public decimal Severity { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal ExpectedValue { get; set; }
        public decimal ActualValue { get; set; }
        public decimal Deviation { get; set; }
        public int? AnalysisId { get; set; }
        public virtual Vehiculo Vehicle { get; set; } = null!;
        public virtual DobackAnalysis? Analysis { get; set; }
    }

    public class AnalysisResult
    {
        public int Id { get; set; }
        public decimal StabilityScore { get; set; }
        public decimal SafetyScore { get; set; }
        public decimal EfficiencyScore { get; set; }
        public decimal MaintenanceScore { get; set; }
        public DateTime AnalysisTime { get; set; }
        public int VehicleId { get; set; }
        public int DobackAnalysisId { get; set; }
        public List<string> Recommendations { get; set; } = new();
        public virtual DobackAnalysis DobackAnalysis { get; set; } = null!;
    }

    public class DashboardMetrics
    {
        public decimal StabilityScore { get; set; }
        public decimal SafetyScore { get; set; }
        public decimal MaintenanceScore { get; set; }
        public int TotalAnalyses { get; set; }
        public DateTime LastAnalysisTime { get; set; }
        public List<Alert> RecentAlerts { get; set; } = new();
        public List<Anomaly> RecentAnomalies { get; set; } = new();
        public TrendMetrics Trends { get; set; } = new();
    }

    public class TrendMetrics
    {
        public TrendData ShortTerm { get; set; } = new();
        public TrendData MediumTerm { get; set; } = new();
        public TrendData LongTerm { get; set; } = new();
    }

    public class DobackData
    {
        public int Id { get; set; }
        public int DobackAnalysisId { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal AccelerationX { get; set; }
        public decimal AccelerationY { get; set; }
        public decimal AccelerationZ { get; set; }
        public decimal Roll { get; set; }
        public decimal Pitch { get; set; }
        public decimal Yaw { get; set; }
        public decimal Speed { get; set; }
        public decimal StabilityIndex { get; set; }
        public decimal Temperature { get; set; }
        public decimal Humidity { get; set; }
        public decimal TimeAntWifi { get; set; }
        public decimal USCycle1 { get; set; }
        public decimal USCycle2 { get; set; }
        public decimal USCycle3 { get; set; }
        public decimal USCycle4 { get; set; }
        public decimal USCycle5 { get; set; }
        public decimal MicrosCleanCAN { get; set; }
        public decimal MicrosSD { get; set; }
        public decimal Steer { get; set; }
        public decimal SafetyScore { get; set; }
        public decimal MaintenanceScore { get; set; }
        public int? CycleId { get; set; }
        public virtual DobackAnalysis DobackAnalysis { get; set; } = null!;

        public decimal GetStabilityScore() =>
            (Math.Abs(Roll) + Math.Abs(Pitch) + Math.Abs(Yaw)) / 3;

        public decimal GetSafetyScore() =>
            SafetyScore;

        public decimal GetMaintenanceScore() =>
            MaintenanceScore;

        public bool IsStable() =>
            Math.Abs(Roll) < 15M && 
            Math.Abs(Pitch) < 15M && 
            Math.Abs(Yaw) < 15M;
    }

    public class AnalysisPrediction
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public DateTime PredictedAt { get; set; }
        public PredictionType Type { get; set; }
        public decimal Probability { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal PredictedValue { get; set; }
        public int? AnalysisId { get; set; }
        public virtual Vehiculo Vehicle { get; set; } = null!;
        public virtual DobackAnalysis? Analysis { get; set; }
    }

    public class PredictionResult
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal PredictedValue { get; set; }
        public decimal ActualValue { get; set; }
        public decimal ErrorMargin { get; set; }
        public string MetricType { get; set; } = string.Empty;
        public int VehicleId { get; set; }
        public virtual Vehiculo Vehicle { get; set; } = null!;
    }

    public class CoreMetrics
    {
        public int TotalAnalyses { get; set; }
        public int TotalAnomalies { get; set; }
        public int TotalPredictions { get; set; }
        public int TotalPatterns { get; set; }
        public decimal AverageStabilityScore { get; set; }
        public decimal AverageSafetyScore { get; set; }
        public decimal AverageMaintenanceScore { get; set; }
        public List<TrendData> StabilityTrend { get; set; } = new();
        public List<TrendData> SafetyTrend { get; set; } = new();
        public List<TrendData> MaintenanceTrend { get; set; } = new();
    }

    // Enums consolidados
    public enum AnalysisType
    {
        Doback,
        Trend,
        Pattern,
        Prediction
    }

    public enum AlertType
    {
        System,
        Maintenance,
        Safety,
        Vehicle,
        Inspection,
        License,
        Performance
    }

    public enum AlertSeverity
    {
        Info,
        Warning,
        Critical
    }

    public enum PredictionType
    {
        Stability,
        Safety,
        Maintenance,
        Performance,
        Efficiency
    }

    public enum PatternType
    {
        Stability,
        Safety,
        Maintenance,
        Performance,
        Efficiency
    }

    public enum AnomalyType
    {
        Stability,
        Safety,
        Maintenance,
        Performance,
        Efficiency,
        System
    }
} 