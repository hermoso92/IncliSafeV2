using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis.Core;

public class AnalysisQueue : BaseEntity
{
    public required int VehicleId { get; set; }
    public required string Name { get; set; }
    public required string Type { get; set; }
    public required string Description { get; set; }
    public required int Priority { get; set; }
    public required bool IsActive { get; set; }
    public required bool IsProcessed { get; set; }
    public required Dictionary<string, string> Parameters { get; set; } = new();
    public required List<AnalysisJob> Jobs { get; set; } = new();
    public required List<AnalysisError> Errors { get; set; } = new();
    public required List<AnalysisNotification> Notifications { get; set; } = new();
} 