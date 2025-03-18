using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis;

public class TrendMetrics
{
    public required decimal StabilityScore { get; set; }
    public required decimal SafetyScore { get; set; }
    public required decimal MaintenanceScore { get; set; }
    public required decimal PerformanceScore { get; set; }
    public required DateTime LastUpdated { get; set; }
    public Dictionary<string, decimal> CustomScores { get; set; } = new();
    public Dictionary<string, object> Parameters { get; set; } = new();
} 