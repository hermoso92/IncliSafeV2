using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis.Core;

public class AnalysisJob : BaseEntity
{
    public required int VehicleId { get; set; }
    public required DateTime ScheduledAt { get; set; }
    public required string Name { get; set; }
    public required string Type { get; set; }
    public required string Description { get; set; }
    public required int RetryCount { get; set; }
    public required int MaxRetries { get; set; }
    public required bool IsCompleted { get; set; }
    public required bool IsFailed { get; set; }
    public required Dictionary<string, string> Parameters { get; set; } = new();
    public required List<AnalysisError> Errors { get; set; } = new();
    public required List<AnalysisNotification> Notifications { get; set; } = new();
} 