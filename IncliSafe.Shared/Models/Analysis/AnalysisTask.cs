using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis;

public class AnalysisTask : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required AnalysisType Type { get; set; }
    public required DateTime ScheduledAt { get; set; }
    public required int VehicleId { get; set; }
    public required TaskStatus Status { get; set; }
    public string? ErrorMessage { get; set; }
    public Dictionary<string, object> Parameters { get; set; } = new();
} 