using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis;

public class AnalysisNotification : BaseEntity
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required AnalysisType Type { get; set; }
    public required NotificationSeverity Severity { get; set; }
    public required bool IsRead { get; set; }
    public required bool IsResolved { get; set; }
    public required Guid VehicleId { get; set; }
    public Dictionary<string, object> Parameters { get; set; } = new();
} 