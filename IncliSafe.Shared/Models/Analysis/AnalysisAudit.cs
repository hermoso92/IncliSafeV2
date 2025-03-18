using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis;

public class AnalysisAudit
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required AnalysisType Type { get; set; }
    public required AuditAction Action { get; set; }
    public required DateTime Timestamp { get; set; }
    public required string UserId { get; set; }
    public string? UserName { get; set; }
    public List<string> AffectedMetrics { get; set; } = new();
    public List<string> AuditNotes { get; set; } = new();
    public Dictionary<string, object> Parameters { get; set; } = new();
} 