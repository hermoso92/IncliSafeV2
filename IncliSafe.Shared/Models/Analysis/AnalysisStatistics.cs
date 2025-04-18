using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis;

public class AnalysisStatistics : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required AnalysisType Type { get; set; }
    public required DateTime GeneratedAt { get; set; }
    public required int TotalAnalyses { get; set; }
    public required int SuccessfulAnalyses { get; set; }
    public required int FailedAnalyses { get; set; }
    public required decimal AverageConfidence { get; set; }
    public required TimeSpan AverageProcessingTime { get; set; }
    public List<string> Metrics { get; set; } = new();
    public Dictionary<string, object> MetricStatistics { get; set; } = new();
    public Dictionary<string, object> Parameters { get; set; } = new();
} 