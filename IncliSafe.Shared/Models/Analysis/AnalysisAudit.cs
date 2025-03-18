using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis;

public class AnalysisAudit : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required AnalysisType Type { get; set; }
    public required AuditAction Action { get; set; }
    public required DateTime Timestamp { get; set; }
    public required string UserId { get; set; }
    public required string UserName { get; set; }
    public List<string> AffectedMetrics { get; set; } = new();
    public string? AuditNotes { get; set; }
    public Dictionary<string, object> Parameters { get; set; } = new();
} 