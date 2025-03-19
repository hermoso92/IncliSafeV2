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
    public required int VehicleId { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public required IncliSafe.Shared.Models.Enums.CycleType Type { get; set; }
    public required decimal Duration { get; set; }
    public required decimal Efficiency { get; set; }
    public required bool IsCompleted { get; set; }
    public List<CycleData> Data { get; set; } = new();
    public Dictionary<string, object> Parameters { get; set; } = new();
    public Dictionary<string, object> Metadata { get; set; } = new();
    public required string Name { get; set; }
    public required string Description { get; set; }
    public DateTime? EndTime { get; set; }
    public bool IsActive => !EndTime.HasValue;
    public List<DobackDataPoint> DataPoints { get; set; } = new();
    public override DateTime CreatedAt { get; set; }
}

public class CycleData
{
    public required DateTime Timestamp { get; set; }
    public required decimal Value { get; set; }
    public string? Label { get; set; }
    public Dictionary<string, decimal> Metrics { get; set; } = new();
} 

