using System;
using System.Collections.Generic;

namespace IncliSafe.Shared.Models.Analysis
{
    public class TrendData
    {
        public DateTime Timestamp { get; set; }
        public decimal Value { get; set; }
        public string Label { get; set; } = string.Empty;
    }

    public class PatternDetails
    {
        public string PatternId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Confidence { get; set; }
        public List<TrendData> Data { get; set; } = new();
    }

    public class PatternHistory
    {
        public string PatternId { get; set; } = string.Empty;
        public DateTime DetectedAt { get; set; }
        public decimal Confidence { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class AnalysisPrediction
    {
        public DateTime Timestamp { get; set; }
        public string PredictionType { get; set; } = string.Empty;
        public decimal Confidence { get; set; }
        public string Details { get; set; } = string.Empty;
        public List<TrendData> TrendData { get; set; } = new();
    }
}