using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis.Core;

public class AnalysisError : BaseEntity
{
    public required int VehicleId { get; set; }
    public required DateTime OccurredAt { get; set; }
    public required string Name { get; set; }
    public required string Type { get; set; }
    public required string Message { get; set; }
    public required string StackTrace { get; set; }
    public required bool IsResolved { get; set; }
    public required Dictionary<string, string> Parameters { get; set; } = new();
    public required List<AnalysisAlert> Alerts { get; set; } = new();
    public required List<AnalysisNotification> Notifications { get; set; } = new();
} 