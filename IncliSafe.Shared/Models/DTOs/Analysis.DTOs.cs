using System;
using System.Collections.Generic;
using System.Linq;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Shared.Models.Analysis.Core;
using IncliSafe.Shared.Models.Enums;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Patterns;

namespace IncliSafe.Shared.Models.DTOs
{
    public class AnalysisDTO : BaseDTO
    {
        public required int VehicleId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required AnalysisType Type { get; set; }
        public required decimal Score { get; set; }
        public required DateTime AnalyzedAt { get; set; }
        public required DateTime AnalysisDate { get; set; }
        public required decimal StabilityScore { get; set; }
        public required decimal SafetyScore { get; set; }
        public required decimal MaintenanceScore { get; set; }
        public required decimal EfficiencyScore { get; set; }
        public string? Notes { get; set; }
        public List<AnalysisDataDTO> DataPoints { get; set; } = new();
        public List<string> Recommendations { get; set; } = new();
        public required AlertSeverity Severity { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = new();
        public required decimal Confidence { get; set; }
        public required AnalysisDataDTO Data { get; set; }
    }

    public class AnalysisDataDTO
    {
        public required DateTime Timestamp { get; set; }
        public required decimal Value { get; set; }
        public required int SensorId { get; set; }
        public string? MetricType { get; set; }
        public Dictionary<string, decimal> AdditionalMetrics { get; set; } = new();
    }

    public class AnomalyDTO : BaseDTO
    {
        public required int VehicleId { get; set; }
        public required DateTime DetectedAt { get; set; }
        public required AnomalyType Type { get; set; }
        public required AlertSeverity Severity { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal Score { get; set; }
        public required decimal ExpectedValue { get; set; }
        public required decimal ActualValue { get; set; }
        public required decimal Deviation { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = new();
        public int? AnalysisId { get; set; }
    }

    public class AnalysisPredictionDTO : BaseDTO
    {
        public required int VehicleId { get; set; }
        public required DateTime PredictedAt { get; set; }
        public required PredictionType Type { get; set; }
        public required decimal Probability { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal PredictedValue { get; set; }
        public required DateTime ValidUntil { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = new();
        public int? AnalysisId { get; set; }
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

    public class TrendAnalysisDTO
    {
        public required decimal StabilityTrend { get; set; }
        public required decimal SafetyTrend { get; set; }
        public required decimal MaintenanceTrend { get; set; }
        public required decimal PerformanceTrend { get; set; }
        public required decimal EfficiencyTrend { get; set; }
        public required decimal ReliabilityTrend { get; set; }
        public required decimal AvailabilityTrend { get; set; }
        public required decimal UtilizationTrend { get; set; }
        public required decimal ComplianceTrend { get; set; }
        public required decimal CostTrend { get; set; }
        public required decimal RiskTrend { get; set; }
        public required decimal QualityTrend { get; set; }
        public required decimal ProductivityTrend { get; set; }
        public required decimal SustainabilityTrend { get; set; }
        public required decimal InnovationTrend { get; set; }
        public required decimal CompetitivenessTrend { get; set; }
        public required decimal CustomerSatisfactionTrend { get; set; }
        public required decimal EmployeeSatisfactionTrend { get; set; }
        public required decimal StakeholderSatisfactionTrend { get; set; }
        public required decimal MarketShareTrend { get; set; }
    }
} 