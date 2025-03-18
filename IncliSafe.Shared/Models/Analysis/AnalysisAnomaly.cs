using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis;

public class AnalysisAnomaly
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required AnomalyType Type { get; set; }
    public required AnomalySeverity Severity { get; set; }
    public required DateTime DetectedAt { get; set; }
    public required decimal Confidence { get; set; }
    public required decimal Value { get; set; }
    public string? Unit { get; set; }
    public decimal? ExpectedValue { get; set; }
    public decimal? Threshold { get; set; }
    public List<string> Recommendations { get; set; } = new();
    public Dictionary<string, object> Parameters { get; set; } = new();
} 