using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.DTOs.Base;
using IncliSafe.Shared.Models.Enums;
using IncliSafe.Shared.Models.Base;
using IncliSafe.Shared.Models.Analysis.Core;

namespace IncliSafe.Shared.Models.DTOs;

public class AnalysisPredictionDTO : BaseDTO
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required PredictionType Type { get; set; }
    public required DateTime PredictedAt { get; set; }
    public required decimal Probability { get; set; }
    public required decimal PredictedValue { get; set; }
    public required DateTime ValidUntil { get; set; }
    public required Dictionary<string, string> Parameters { get; set; } = new();
    public required PredictionRisk Risk { get; set; }
    public required bool IsActive { get; set; }
    public required DateTime UpdatedAt { get; set; }
    public required string Resolution { get; set; }
    public required string ModelVersion { get; set; }
} 