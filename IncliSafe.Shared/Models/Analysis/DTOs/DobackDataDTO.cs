using IncliSafe.Shared.Models.Base;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis.DTOs;

public class DobackDataDTO : BaseDTO
{
    public required int VehicleId { get; set; }
    public required DateTime Timestamp { get; set; }
    public required SensorType Type { get; set; }
    public required decimal Value { get; set; }
    public required string Unit { get; set; }
    public required bool IsValid { get; set; }
    public required bool IsProcessed { get; set; }
    public required string Resolution { get; set; }
    public required Dictionary<string, string> Parameters { get; set; }
} 