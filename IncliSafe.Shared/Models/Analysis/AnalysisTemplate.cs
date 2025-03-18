using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis;

public class AnalysisTemplate : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required AnalysisType Type { get; set; }
    public required decimal DefaultConfidenceThreshold { get; set; }
    public required int DefaultSampleSize { get; set; }
    public List<string> DefaultMetrics { get; set; } = new();
    public List<string> RequiredMetrics { get; set; } = new();
    public Dictionary<string, object> DefaultParameters { get; set; } = new();
    public required bool IsEnabled { get; set; }
    public required DateTime LastModified { get; set; }
} 