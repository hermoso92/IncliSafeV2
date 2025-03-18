using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis;

public class DobackAnalysis : BaseEntity
{
    public override Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required AnalysisType Type { get; set; }
    public decimal StabilityScore { get; set; }
    public required Guid VehicleId { get; set; }
    public List<DobackDataPoint> DataPoints { get; set; } = new();
    public List<string> Recommendations { get; set; } = new();
    public List<string> Notes { get; set; } = new();
    public Dictionary<string, object> Parameters { get; set; } = new();
    public DateTime DetectedAt { get; set; }
    public AlertSeverity Severity { get; set; }
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