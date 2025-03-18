using IncliSafe.Shared.Models.Enums;
using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Analysis;

namespace IncliSafe.Shared.Models.Analysis;

public class TrendAnalysisEntity
{
    public required int Id { get; set; }
    public required int VehicleId { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public required decimal TrendValue { get; set; }
    public required decimal Seasonality { get; set; }
    public required decimal Correlation { get; set; }
    public List<TrendData> Data { get; set; } = new();
    public virtual Vehiculo Vehicle { get; set; } = null!;
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required decimal Value { get; set; }
    public required string Direction { get; set; } = string.Empty;
    public required DateTime DetectedAt { get; set; }
} 

