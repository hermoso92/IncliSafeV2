using System;
using System.Collections.Generic;

namespace IncliSafe.Shared.Models.Analysis
{
    public class DobackAnalysis
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public DateTime Timestamp { get; set; }
        public string FileName { get; set; } = string.Empty;
        public decimal StabilityIndex { get; set; }
        public decimal SafetyIndex { get; set; }
        public decimal EfficiencyIndex { get; set; }
        public List<string> Recommendations { get; set; } = new();
        public List<TrendData> TrendData { get; set; } = new();
        public List<PatternDetails> DetectedPatterns { get; set; } = new();
        public List<Anomaly> DetectedAnomalies { get; set; } = new();
    }

    public class TrendData
    {
        public DateTime Timestamp { get; set; }
        public decimal Value { get; set; }
        public string Label { get; set; } = string.Empty;
    }

    public class PatternDetails
    {
        public string PatternId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Confidence { get; set; }
        public List<TrendData> Data { get; set; } = new();
    }

    public class PatternHistory
    {
        public string PatternId { get; set; } = string.Empty;
        public DateTime DetectedAt { get; set; }
        public decimal Confidence { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class AnalysisPrediction
    {
        public DateTime Timestamp { get; set; }
        public string PredictionType { get; set; } = string.Empty;
        public decimal Confidence { get; set; }
        public string Details { get; set; } = string.Empty;
        public List<TrendData> TrendData { get; set; } = new();
    }

    public class AnalysisResult
    {
        public decimal StabilityScore { get; set; }
        public decimal SafetyScore { get; set; }
        public decimal EfficiencyScore { get; set; }
        public decimal MaintenanceScore { get; set; }
        public DateTime AnalysisTime { get; set; }
        public int VehicleId { get; set; }
        public List<string> Recommendations { get; set; } = new();
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
    }

    public class Anomaly
    {
        public int Id { get; set; }
        public DateTime DetectedAt { get; set; }
        public string Type { get; set; } = string.Empty;
        public decimal Severity { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class Pattern
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Confidence { get; set; }
        public DateTime DetectedAt { get; set; }
    }

    public class PredictionResult
    {
        public decimal PredictedValue { get; set; }
        public decimal Confidence { get; set; }
        public DateTime PredictionTime { get; set; }
        public string Details { get; set; } = string.Empty;
    }

    public class TrendAnalysis
    {
        public List<TrendData> Data { get; set; } = new();
        public decimal Trend { get; set; }
        public decimal Seasonality { get; set; }
        public decimal Correlation { get; set; }
        public DateTime AnalysisTime { get; set; }
    }

    public enum PredictionType
    {
        Stability,
        Safety,
        Maintenance,
        Performance,
        Anomaly
    }
} 