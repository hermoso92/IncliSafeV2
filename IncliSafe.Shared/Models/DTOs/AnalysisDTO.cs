using IncliSafe.Shared.Models.DTOs.Base;
using IncliSafe.Shared.Models.Enums;
using System;
using System.Collections.Generic;

namespace IncliSafe.Shared.Models.DTOs;

public class AnalysisDTO : BaseDTO
{
    public required string VehicleId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string Type { get; set; }
    public required decimal Score { get; set; }
    public required DateTime AnalyzedAt { get; set; }
    public required DateTime AnalysisDate { get; set; }
    public required decimal StabilityScore { get; set; }
    public required decimal SafetyScore { get; set; }
    public required decimal MaintenanceScore { get; set; }
    public required decimal EfficiencyScore { get; set; }
    public string? Notes { get; set; }
    public required List<DataPointDTO> DataPoints { get; set; } = new();
    public required List<string> Recommendations { get; set; } = new();
    public required AnalysisSeverity Severity { get; set; }
    public required Dictionary<string, string> Parameters { get; set; } = new();
    public required decimal Confidence { get; set; }
    public required AnalysisDataDTO Data { get; set; }
    public required AnalysisType AnalysisType { get; set; }
    public required TimeSpan AnalysisTime { get; set; }
    public required int DobackAnalysisId { get; set; }
} 