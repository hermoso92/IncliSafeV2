using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Patterns;

namespace IncliSafe.Shared.Models.Analysis
{
    public class TrendData
    {
        public int Id { get; set; }
        public List<DateTime> TimePoints { get; set; } = new();
        public List<double> Values { get; set; } = new();
        public string Type { get; set; } = string.Empty;
        public double Confidence { get; set; }
        public int AnalysisId { get; set; }
        public List<DataPoint> Points { get; set; } = new();
        public List<TrendMetric> Metrics { get; set; } = new();
        public List<Prediction> Predictions { get; set; } = new();
        public List<DetectedPattern> Patterns { get; set; } = new();
    }

    public class DataPoint
    {
        public DateTime Timestamp { get; set; }
        public decimal Value { get; set; }
    }
} 