using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis.Core;

public class AnalysisStatistics : BaseEntity
{
    public required int VehicleId { get; set; }
    public required DateTime GeneratedAt { get; set; }
    public required string Type { get; set; }
    public required string Description { get; set; }
    public required decimal Score { get; set; }
    public required decimal Confidence { get; set; }
    public required bool IsActive { get; set; }
    public required bool IsValidated { get; set; }
    public required Dictionary<string, string> Parameters { get; set; } = new();
    public required List<AnalysisAlert> Alerts { get; set; } = new();
    public required List<AnalysisNotification> Notifications { get; set; } = new();
} 