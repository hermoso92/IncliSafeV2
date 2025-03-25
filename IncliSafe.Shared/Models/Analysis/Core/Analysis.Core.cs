using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IncliSafe.Shared.Models.DTOs;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Enums;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Analysis.Data;
using IncliSafe.Shared.Models.Notifications;

namespace IncliSafe.Shared.Models.Analysis.Core;

public interface IAnalysisService
{
    Task<AnalysisResult> AnalyzeDataAsync(AnalysisResult analysis);
    Task<List<Anomaly>> DetectAnomaliesAsync(AnalysisResult analysis);
    Task<List<AnalysisPrediction>> GeneratePredictionsAsync(AnalysisResult analysis);
    Task<List<TrendInfo>> AnalyzeTrendsAsync(AnalysisResult analysis);
}

public interface IAnalysisRepository
{
    Task<AnalysisResult> GetAnalysisAsync(int id);
    Task<List<AnalysisResult>> GetAnalysesAsync(int vehicleId);
    Task<AnalysisResult> CreateAnalysisAsync(AnalysisResult analysis);
    Task<AnalysisResult> UpdateAnalysisAsync(AnalysisResult analysis);
    Task<bool> DeleteAnalysisAsync(int id);
}

public interface IAnalysisFactory
{
    AnalysisResult CreateAnalysis(string name, AnalysisType type, int vehicleId);
    AnalysisResult CreateAnalysis(AnalysisDTO dto);
    AnalysisResult UpdateAnalysis(AnalysisResult analysis, AnalysisDTO dto);
    AnalysisResult CloneAnalysis(AnalysisResult analysis);
    AnalysisResult MergeAnalyses(AnalysisResult analysis1, AnalysisResult analysis2);
}

public abstract class AnalysisBase
{
    public int Id { get; set; }
    public required virtual string Type { get; set; }
    public required string Description { get; set; }
    public required string Resolution { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public required string ModelVersion { get; set; }
}

public class AnalysisData
{
    public required DateTime Timestamp { get; set; }
    public required decimal Value { get; set; }
    public required int SensorId { get; set; }
    public string? MetricType { get; set; }
    public Dictionary<string, decimal> AdditionalMetrics { get; set; } = new();
}

public class DetectedPattern
{
    public required string Type { get; set; }
    public string? Description { get; set; }
    public required decimal Confidence { get; set; }
    public List<CorePatternDataPoint> DataPoints { get; set; } = new();
    public Dictionary<string, string> Metadata { get; set; } = new();
}

public class CorePatternDataPoint
{
    public required DateTime Timestamp { get; set; }
    public string? Label { get; set; }
    public required decimal Value { get; set; }
    public Dictionary<string, decimal> Metrics { get; set; } = new();
}

public class DashboardMetrics
{
    public required decimal StabilityScore { get; set; }
    public required decimal SafetyScore { get; set; }
    public required decimal MaintenanceScore { get; set; }
    public required int TotalAnalyses { get; set; }
    public required DateTime LastAnalysisTime { get; set; }
    public List<Notifications.Alert> RecentAlerts { get; set; } = new();
    public List<Anomaly> RecentAnomalies { get; set; } = new();
    public required TrendMetrics Trends { get; set; }
}

public class TrendMetrics
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

public class PredictionTypeInfo
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required PredictionType Type { get; set; }
    public required decimal Confidence { get; set; }
}

public class PatternTypeInfo
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required PatternType Type { get; set; }
    public required decimal Confidence { get; set; }
}

public class CoreMetrics
{
    public int Id { get; set; }
    public decimal AverageInclination { get; set; }
    public decimal MaxInclination { get; set; }
    public decimal MinInclination { get; set; }
    public decimal StandardDeviation { get; set; }
    public int TotalAnalyses { get; set; }
    public DateTime LastAnalysisTime { get; set; }
    public List<string> RecentAlerts { get; set; } = new();
}

public class Anomaly : AnalysisBase
{
    public required string Name { get; set; }
    public double Score { get; set; }
    public required object ExpectedValue { get; set; }
    public required object ActualValue { get; set; }
    public override required string Type { get; set; }
    public required string Severity { get; set; }
    public DateTime DetectedAt { get; set; }
    public bool IsActive { get; set; }
    public Dictionary<string, object> Parameters { get; set; } = new();
}

public class Analysis : AnalysisBase
{
    public required string Name { get; set; }
    public decimal Score { get; set; }
    public decimal Value { get; set; }
    public decimal Threshold { get; set; }
    public override required string Type { get; set; }
    public Dictionary<string, string> Metadata { get; set; } = new();
} 