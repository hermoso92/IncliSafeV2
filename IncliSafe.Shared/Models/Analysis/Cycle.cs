using IncliSafe.Shared.Models.Enums;
using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafe.Shared.Models.Analysis;

public class Cycle
{
    public required int Id { get; set; }
    public required int VehicleId { get; set; }
    public required DateTime StartTime { get; set; }
    public required DateTime EndTime { get; set; }
    public required decimal Duration { get; set; }
    public required decimal Distance { get; set; }
    public required decimal AverageSpeed { get; set; }
    public required decimal MaxSpeed { get; set; }
    public required decimal AverageInclination { get; set; }
    public required decimal MaxInclination { get; set; }
    public required decimal StabilityScore { get; set; }
    public required decimal SafetyScore { get; set; }
    public required decimal MaintenanceScore { get; set; }
    public virtual Vehiculo Vehicle { get; set; } = null!;
    public List<DobackData> Data { get; set; } = new();
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required decimal Value { get; set; }
    public required DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public bool IsCompleted => EndedAt.HasValue;
} 

