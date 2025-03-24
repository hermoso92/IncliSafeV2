using IncliSafe.Shared.Models.Base;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis.Core;

public class DobackData : BaseEntity
{
    public required int VehicleId { get; set; }
    public required DateTime Timestamp { get; set; }
    public required SensorType Type { get; set; }
    public required decimal Value { get; set; }
    public required string Unit { get; set; }
    public required bool IsValid { get; set; }
    public required bool IsProcessed { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
    public required string Resolution { get; set; }
    public required Dictionary<string, string> Parameters { get; set; } = new();
} 