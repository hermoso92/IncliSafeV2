using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis;

public class AnalysisPrediction
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required PredictionType Type { get; set; }
    public required DateTime PredictedAt { get; set; }
    public required decimal Confidence { get; set; }
    public required decimal Value { get; set; }
    public string? Unit { get; set; }
    public decimal? LowerBound { get; set; }
    public decimal? UpperBound { get; set; }
    public List<string> Factors { get; set; } = new();
    public Dictionary<string, object> Parameters { get; set; } = new();
} 