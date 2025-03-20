using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis;

public class AnalysisNotification : BaseEntity
{
    public required string Title { get; set; }
    public required string Message { get; set; }
    public required NotificationType Type { get; set; }
    public required NotificationSeverity Severity { get; set; }
    public required int VehicleId { get; set; }
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }
    public string? ReadBy { get; set; }
    public Dictionary<string, object> Parameters { get; set; } = new();
} 