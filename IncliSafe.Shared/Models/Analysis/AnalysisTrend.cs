using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis;

public class AnalysisTrend
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required TrendType Type { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public required decimal Value { get; set; }
    public string? Unit { get; set; }
    public required decimal Confidence { get; set; }
    public List<decimal> HistoricalValues { get; set; } = new();
    public List<DateTime> HistoricalDates { get; set; } = new();
    public Dictionary<string, object> Parameters { get; set; } = new();
} 