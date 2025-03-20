using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis.Core
{
    public class DobackAnalysis : BaseEntity
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal StabilityIndex { get; set; }
        public required decimal SafetyIndex { get; set; }
        public required decimal MaintenanceIndex { get; set; }
        public required int VehicleId { get; set; }
        public required DateTime AnalysisDate { get; set; }
        public required AnalysisType Type { get; set; }
        public List<DobackData> Data { get; set; } = new();
        public List<Anomaly> Anomalies { get; set; } = new();
        public List<AnalysisPrediction> Predictions { get; set; } = new();
        public List<DetectedPattern> Patterns { get; set; } = new();
        public Dictionary<string, object> Parameters { get; set; } = new();
        public string? Notes { get; set; }
    }

    public class DobackData
    {
        public required DateTime Timestamp { get; set; }
        public required decimal Value { get; set; }
        public required int SensorId { get; set; }
        public string? MetricType { get; set; }
        public Dictionary<string, decimal> AdditionalMetrics { get; set; } = new();
    }

    public class CoreMetrics
    {
        public required decimal StabilityScore { get; set; }
        public required decimal SafetyScore { get; set; }
        public required decimal MaintenanceScore { get; set; }
        public required decimal PerformanceScore { get; set; }
        public required decimal EfficiencyScore { get; set; }
        public required decimal ReliabilityScore { get; set; }
        public required decimal AvailabilityScore { get; set; }
        public required decimal UtilizationScore { get; set; }
        public required decimal ComplianceScore { get; set; }
        public required decimal CostScore { get; set; }
        public required decimal RiskScore { get; set; }
        public required decimal QualityScore { get; set; }
        public required decimal ProductivityScore { get; set; }
        public required decimal SustainabilityScore { get; set; }
        public required decimal InnovationScore { get; set; }
        public required decimal CompetitivenessScore { get; set; }
        public required decimal CustomerSatisfactionScore { get; set; }
        public required decimal EmployeeSatisfactionScore { get; set; }
        public required decimal StakeholderSatisfactionScore { get; set; }
        public required decimal MarketShareScore { get; set; }
    }
} 