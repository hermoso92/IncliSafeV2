using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Enums;
using IncliSafe.Shared.Models.Analysis.Core;

namespace IncliSafe.Shared.Models.Analysis;

public class DobackAnalysis : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required AnalysisType Type { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public required decimal Confidence { get; set; }
    public List<DobackDataPoint> DataPoints { get; set; } = new();
    public Dictionary<string, object> Parameters { get; set; } = new();
}

public class DobackDataPoint
{
    public required DateTime Timestamp { get; set; }
    public required decimal Value { get; set; }
    public string? Unit { get; set; }
    public decimal? ExpectedValue { get; set; }
    public decimal? Threshold { get; set; }
    public decimal? Deviation { get; set; }
    public Dictionary<string, object> Parameters { get; set; } = new();
}

public class DobackData
{
    public required DateTime Timestamp { get; set; }
    public required decimal Value { get; set; }
    public string? Unit { get; set; }
    public Dictionary<string, object> Metadata { get; set; } = new();
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