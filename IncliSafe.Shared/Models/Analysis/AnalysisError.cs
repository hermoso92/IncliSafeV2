using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis;

public class AnalysisError
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required AnalysisType Type { get; set; }
    public required ErrorSeverity Severity { get; set; }
    public required DateTime OccurredAt { get; set; }
    public required string ErrorCode { get; set; }
    public required string ErrorMessage { get; set; }
    public string? StackTrace { get; set; }
    public List<string> AffectedMetrics { get; set; } = new();
    public List<string> ResolutionSteps { get; set; } = new();
    public Dictionary<string, object> Parameters { get; set; } = new();
} 