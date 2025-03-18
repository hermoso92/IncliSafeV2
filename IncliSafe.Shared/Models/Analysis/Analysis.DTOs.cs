using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Notifications;
using IncliSafe.Shared.Models.Analysis.Core;

namespace IncliSafe.Shared.Models.Analysis
{
    public class AnalysisDTO
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
        public virtual DobackAnalysisDTO DobackAnalysis { get; set; } = null!;
    }

    public class DobackAnalysisDTO
    {
        public required int Id { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal StabilityIndex { get; set; }
        public required decimal SafetyIndex { get; set; }
        public required decimal MaintenanceIndex { get; set; }
        public List<DobackDataDTO> Data { get; set; } = new();
        public List<AnomalyDTO> Anomalies { get; set; } = new();
        public List<AnalysisPredictionDTO> Predictions { get; set; } = new();
        public List<DetectedPatternDTO> Patterns { get; set; } = new();
    }

    public class DobackDataDTO
    {
        public required DateTime Timestamp { get; set; }
        public required decimal Value { get; set; }
        public required int SensorId { get; set; }
        public string? MetricType { get; set; }
        public Dictionary<string, decimal> AdditionalMetrics { get; set; } = new();
    }

    public class AnomalyDTO
    {
        public required int Id { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required int VehicleId { get; set; }
        public required DateTime DetectedAt { get; set; }
        public required AnomalyType Type { get; set; }
        public required AlertSeverity Severity { get; set; }
        public required string Description { get; set; } = string.Empty;
        public required decimal ExpectedValue { get; set; }
        public required decimal ActualValue { get; set; }
        public required decimal Deviation { get; set; }
        public int? AnalysisId { get; set; }
        public virtual VehicleDTO Vehicle { get; set; } = null!;
        public virtual DobackAnalysisDTO? Analysis { get; set; }
    }

    public class AnalysisPredictionDTO
    {
        public required int Id { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required int VehicleId { get; set; }
        public required DateTime PredictedAt { get; set; }
        public required PredictionType Type { get; set; }
        public required decimal Probability { get; set; }
        public required string Description { get; set; } = string.Empty;
        public required decimal PredictedValue { get; set; }
        public int? AnalysisId { get; set; }
        public virtual VehicleDTO Vehicle { get; set; } = null!;
        public virtual DobackAnalysisDTO? Analysis { get; set; }
    }

    public class DetectedPatternDTO
    {
        public required string Type { get; set; }
        public string? Description { get; set; }
        public required decimal Confidence { get; set; }
        public List<PatternDataPointDTO> DataPoints { get; set; } = new();
        public Dictionary<string, string> Metadata { get; set; } = new();
    }

    public class PatternDataPointDTO
    {
        public required DateTime Timestamp { get; set; }
        public string? Label { get; set; }
        public required decimal Value { get; set; }
        public Dictionary<string, decimal> Metrics { get; set; } = new();
    }

    public class VehicleDTO
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Type { get; set; }
        public required string Status { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required bool IsActive { get; set; }
    }
} 