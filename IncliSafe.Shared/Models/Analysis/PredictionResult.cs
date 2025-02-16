using System;
using System.Collections.Generic;

namespace IncliSafe.Shared.Models.Analysis
{
    public class PredictionResult
    {
        public int Id { get; set; }
        public List<DateTime> TimePoints { get; set; } = new();
        public List<double> Predictions { get; set; } = new();
        public List<double> UpperBound { get; set; } = new();
        public List<double> LowerBound { get; set; } = new();
        public string Trend { get; set; } = string.Empty;
        public List<string> Recommendations { get; set; } = new();
        public double Confidence { get; set; }
    }
} 