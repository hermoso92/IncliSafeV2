using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis;

public class AnalysisExport
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required AnalysisType Type { get; set; }
    public required DateTime ExportedAt { get; set; }
    public required string Format { get; set; }
    public required string FilePath { get; set; }
    public long FileSize { get; set; }
    public string? MimeType { get; set; }
    public List<string> IncludedMetrics { get; set; } = new();
    public Dictionary<string, object> Parameters { get; set; } = new();
} 