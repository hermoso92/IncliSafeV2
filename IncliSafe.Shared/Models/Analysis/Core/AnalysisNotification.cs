using IncliSafe.Shared.Models.Base;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis.Core;

public class AnalysisNotification : BaseEntity
{
    public required int VehicleId { get; set; }
    public required DateTime GeneratedAt { get; set; }
    public required NotificationType Type { get; set; }
    public required string Title { get; set; }
    public required string Message { get; set; }
    public required NotificationSeverity Severity { get; set; }
    public required bool IsActive { get; set; }
    public required bool IsRead { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
    public required string Resolution { get; set; }
    public required Dictionary<string, string> Parameters { get; set; } = new();
    public required List<DobackData> RelatedData { get; set; } = new();
} 