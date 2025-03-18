using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis;

public class AnalysisReport : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required AnalysisType Type { get; set; }
    public required ReportFormat Format { get; set; }
    public required DateTime GeneratedAt { get; set; }
    public required string FilePath { get; set; }
    public required long FileSize { get; set; }
    public string? Checksum { get; set; }
    public List<string> IncludedMetrics { get; set; } = new();
    public Dictionary<string, object> Parameters { get; set; } = new();
} 