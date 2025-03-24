using IncliSafe.Shared.Models.DTOs.Base;
using IncliSafe.Shared.Models.Enums;
using System;
using System.Collections.Generic;

namespace IncliSafe.Shared.Models.DTOs;

public class DobackAnalysisDTO : BaseDTO
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required decimal StabilityScore { get; set; }
    public required decimal SafetyScore { get; set; }
    public required decimal MaintenanceScore { get; set; }
    public required int VehicleId { get; set; }
    public required DateTime AnalysisDate { get; set; }
    public required AnalysisType Type { get; set; }
    public required List<DobackDataDto> Data { get; set; } = new();
    public required List<AnomalyDTO> Anomalies { get; set; } = new();
    public required List<AnalysisPredictionDTO> Predictions { get; set; } = new();
    public required List<DataPointDTO> Patterns { get; set; } = new();
    public required Dictionary<string, object> Parameters { get; set; } = new();
    public string? Notes { get; set; }
} 