using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis;

public class AnalysisPattern : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required AnalysisType Type { get; set; }
    public required bool IsEnabled { get; set; }
    public required string Pattern { get; set; }
    public required decimal Confidence { get; set; }
    public List<string> AffectedMetrics { get; set; } = new();
    public Dictionary<string, object> Parameters { get; set; } = new();
} 