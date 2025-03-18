using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis;

public class AnalysisFilter
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required AnalysisType Type { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? MinConfidence { get; set; }
    public decimal? MaxConfidence { get; set; }
    public List<string> Metrics { get; set; } = new();
    public List<string> ExcludedMetrics { get; set; } = new();
    public Dictionary<string, object> Parameters { get; set; } = new();
} 