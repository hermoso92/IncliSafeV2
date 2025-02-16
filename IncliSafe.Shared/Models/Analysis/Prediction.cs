using System;

namespace IncliSafe.Shared.Models.Analysis
{
    public class Prediction
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public double Value { get; set; }
        public double Confidence { get; set; }
        public string Type { get; set; } = string.Empty;
        public double RiskScore { get; set; }
        public int TrendAnalysisId { get; set; }
        public virtual TrendAnalysis? TrendAnalysis { get; set; }
        public int? DobackAnalysisId { get; set; }
        public virtual DobackAnalysis? DobackAnalysis { get; set; }
    }
} 