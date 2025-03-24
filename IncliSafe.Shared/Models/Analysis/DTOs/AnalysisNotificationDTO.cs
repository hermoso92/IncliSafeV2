using IncliSafe.Shared.Models.Base;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis.DTOs;

public class AnalysisNotificationDTO : BaseDTO
{
    public required int VehicleId { get; set; }
    public required DateTime GeneratedAt { get; set; }
    public required NotificationType Type { get; set; }
    public required string Title { get; set; }
    public required string Message { get; set; }
    public required NotificationSeverity Severity { get; set; }
    public required bool IsActive { get; set; }
    public required bool IsRead { get; set; }
    public required string Resolution { get; set; }
    public required Dictionary<string, string> Parameters { get; set; }
} 