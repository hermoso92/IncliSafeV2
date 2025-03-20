using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis;

public class AnalysisAlert : BaseEntity
{
    public required string Title { get; set; }
    public required string Message { get; set; }
    public required AlertType Type { get; set; }
    public required AlertSeverity Severity { get; set; }
    public required AlertPriority Priority { get; set; }
    public required int VehicleId { get; set; }
    public bool IsResolved { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public string? ResolvedBy { get; set; }
    public Dictionary<string, object> Parameters { get; set; } = new();
} 