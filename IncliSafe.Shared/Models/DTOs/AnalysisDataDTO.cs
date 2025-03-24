using IncliSafe.Shared.Models.DTOs.Base;
using System;
using System.Collections.Generic;

namespace IncliSafe.Shared.Models.DTOs;

public class AnalysisDataDTO : BaseDTO
{
    public required DateTime Timestamp { get; set; }
    public required decimal Value { get; set; }
    public required int SensorId { get; set; }
    public string? MetricType { get; set; }
    public Dictionary<string, decimal> AdditionalMetrics { get; set; } = new();
} 