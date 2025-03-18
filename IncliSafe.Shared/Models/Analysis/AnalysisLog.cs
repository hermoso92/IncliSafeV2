using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis;

public class AnalysisLog
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required AnalysisType Type { get; set; }
    public required LogLevel Level { get; set; }
    public required DateTime Timestamp { get; set; }
    public required string Message { get; set; }
    public string? Exception { get; set; }
    public string? StackTrace { get; set; }
    public List<string> AffectedMetrics { get; set; } = new();
    public List<string> Tags { get; set; } = new();
    public Dictionary<string, object> Parameters { get; set; } = new();
} 