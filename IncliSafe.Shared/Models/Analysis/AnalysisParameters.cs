using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis;

public class AnalysisParameters : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required AnalysisType Type { get; set; }
    public required bool IsEnabled { get; set; }
    public required decimal ConfidenceThreshold { get; set; }
    public required int SampleSize { get; set; }
    public required int MaxRetries { get; set; }
    public required TimeSpan Timeout { get; set; }
    public List<string> RequiredMetrics { get; set; } = new();
    public Dictionary<string, object> Parameters { get; set; } = new();
} 