using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis.Core;

public class AnalysisAlert : BaseEntity
{
    public required int VehicleId { get; set; }
    public required DateTime GeneratedAt { get; set; }
    public required AlertType Type { get; set; }
    public required string Title { get; set; }
    public required string Message { get; set; }
    public required AlertSeverity Severity { get; set; }
    public required bool IsActive { get; set; }
    public required bool IsAcknowledged { get; set; }
    public required Dictionary<string, string> Parameters { get; set; } = new();
    public required List<DobackData> RelatedData { get; set; } = new();
    public required List<AnalysisNotification> Notifications { get; set; } = new();
} 