using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis;

public class AnalysisResult
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required AnalysisType Type { get; set; }
    public required DateTime AnalysisDate { get; set; }
    public required decimal Confidence { get; set; }
    public List<Anomaly> Anomalies { get; set; } = new();
    public List<Prediction> Predictions { get; set; } = new();
    public List<TrendData> Trends { get; set; } = new();
    public List<string> Recommendations { get; set; } = new();
    public Dictionary<string, object> Parameters { get; set; } = new();
} 