using System;

namespace IncliSafe.Shared.Models.Analysis
{
    public class Prediction
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public DateTime Timestamp { get; set; }
        public PredictionType Type { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Confidence { get; set; }
        public PredictionRisk Risk { get; set; }
        public int AnalysisId { get; set; }
        public virtual DobackAnalysis Analysis { get; set; } = null!;
    }

    public enum PredictionType
    {
        Stability,
        Safety,
        Maintenance,
        Performance
    }

    public enum PredictionRisk
    {
        Low,
        Medium,
        High,
        Critical
    }
} 