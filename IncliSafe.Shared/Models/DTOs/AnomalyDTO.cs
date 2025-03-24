using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.DTOs.Base;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.DTOs;

public class AnomalyDTO : BaseDTO
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required decimal Score { get; set; }
    public required AnalysisSeverity Severity { get; set; }
    public required AnomalyType Type { get; set; }
    public required DateTime DetectedAt { get; set; }
    public required decimal ExpectedValue { get; set; }
    public required decimal ActualValue { get; set; }
    public required decimal Deviation { get; set; }
    public required Dictionary<string, string> Parameters { get; set; } = new();
    public required string Resolution { get; set; }
    public required string ModelVersion { get; set; }
} 