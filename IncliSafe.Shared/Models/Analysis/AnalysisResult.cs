using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis;

public class AnalysisResult : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required AnalysisType Type { get; set; }
    public required decimal Score { get; set; }
    public required DateTime AnalyzedAt { get; set; }
    public required int VehicleId { get; set; }
    public List<AnalysisData> DataPoints { get; set; } = new();
    public List<string> Recommendations { get; set; } = new();
    public string? Notes { get; set; }
    public required AlertSeverity Severity { get; set; }
    public Dictionary<string, object> Parameters { get; set; } = new();
} 