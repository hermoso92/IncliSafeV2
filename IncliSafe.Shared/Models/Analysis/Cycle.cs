using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Entities;
using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Shared.Models.DTOs;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis;

public class Cycle : BaseEntity
{
    public override Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required CycleType Type { get; set; }
    public required Guid VehicleId { get; set; }
    public required DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public bool IsActive => !EndTime.HasValue;
    public List<DobackDataPoint> DataPoints { get; set; } = new();
    public Dictionary<string, object> Parameters { get; set; } = new();
    public override DateTime CreatedAt { get; set; }
} 

