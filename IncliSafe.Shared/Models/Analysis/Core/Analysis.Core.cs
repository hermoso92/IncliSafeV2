using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IncliSafe.Shared.Models.DTOs;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Enums;
using IncliSafe.Shared.Models.Analysis;

namespace IncliSafe.Shared.Models.Analysis.Core;

public interface IAnalysisService
{
    Task<DobackAnalysis> AnalyzeDataAsync(DobackAnalysis analysis);
    Task<List<Anomaly>> DetectAnomaliesAsync(DobackAnalysis analysis);
    Task<List<AnalysisPrediction>> GeneratePredictionsAsync(DobackAnalysis analysis);
    Task<List<TrendInfo>> AnalyzeTrendsAsync(DobackAnalysis analysis);
}

public interface IAnalysisRepository
{
    Task<DobackAnalysis> GetAnalysisAsync(Guid id);
    Task<List<DobackAnalysis>> GetAnalysesAsync(Guid vehicleId);
    Task<DobackAnalysis> CreateAnalysisAsync(DobackAnalysis analysis);
    Task<DobackAnalysis> UpdateAnalysisAsync(DobackAnalysis analysis);
    Task<bool> DeleteAnalysisAsync(Guid id);
}

public interface IAnalysisFactory
{
    DobackAnalysis CreateAnalysis(string name, AnalysisType type, Guid vehicleId);
    DobackAnalysis CreateAnalysis(DobackAnalysisDTO dto);
    DobackAnalysis UpdateAnalysis(DobackAnalysis analysis, DobackAnalysisDTO dto);
    DobackAnalysis CloneAnalysis(DobackAnalysis analysis);
    DobackAnalysis MergeAnalyses(DobackAnalysis analysis1, DobackAnalysis analysis2);
}

public class AnalysisPrediction : BaseEntity
{
    public override Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required PredictionType Type { get; set; }
    public required decimal Confidence { get; set; }
    public required DateTime PredictedAt { get; set; }
    public required DateTime ValidUntil { get; set; }
    public Dictionary<string, object> Parameters { get; set; } = new();
}

public class AnalysisResult : BaseEntity
{
    public override Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required AnalysisType Type { get; set; }
    public required decimal Score { get; set; }
    public required DateTime AnalyzedAt { get; set; }
    public Dictionary<string, object> Parameters { get; set; } = new();
}

public class Anomaly : BaseEntity
{
    public override Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required AnomalyType Type { get; set; }
    public required decimal Score { get; set; }
    public required DateTime DetectedAt { get; set; }
    public required AlertSeverity Severity { get; set; }
    public Dictionary<string, object> Parameters { get; set; } = new();
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



