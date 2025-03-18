using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis;

public class AnalysisNotification
{
    public required Guid Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required AnalysisType Type { get; set; }
    public required NotificationSeverity Severity { get; set; }
    public required DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
    public bool IsResolved { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public string? Resolution { get; set; }
    public List<string> AffectedMetrics { get; set; } = new();
    public Dictionary<string, object> Parameters { get; set; } = new();
} 