using System;
using IncliSafe.Shared.Models.Analysis.Core;

public class AnalysisPrediction
{
    public int Id { get; set; }
    public int AnalysisId { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public decimal Value { get; set; }
    public decimal Confidence { get; set; }
    public PredictionType Type { get; set; }
} 