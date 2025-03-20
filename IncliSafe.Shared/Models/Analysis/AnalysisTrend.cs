using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis;

public class TrendDataPoint
{
    public required DateTime Timestamp { get; set; }
    public required decimal Value { get; set; }
    public string? Label { get; set; }
    public Dictionary<string, decimal> Metrics { get; set; } = new();
}

public class AnalysisTrend : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required TrendType Type { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public required decimal Value { get; set; }
    public required decimal Change { get; set; }
    public required decimal PercentageChange { get; set; }
    public required TrendDirection Direction { get; set; }
    public required decimal Confidence { get; set; }
    public List<TrendDataPoint> DataPoints { get; set; } = new();
    public Dictionary<string, object> Parameters { get; set; } = new();
} 