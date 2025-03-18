using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis;

public class AnalysisValidation
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required AnalysisType Type { get; set; }
    public required ValidationStatus Status { get; set; }
    public required DateTime ValidatedAt { get; set; }
    public string? ValidatedBy { get; set; }
    public List<string> ValidatedMetrics { get; set; } = new();
    public List<string> ValidationNotes { get; set; } = new();
    public Dictionary<string, object> Parameters { get; set; } = new();
} 