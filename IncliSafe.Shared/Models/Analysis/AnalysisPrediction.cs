using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Analysis.Core;

namespace IncliSafe.Shared.Models.Analysis
{
    public class AnalysisPrediction
    {
        public int Id { get; set; }
        public int AnalysisId { get; set; }
        public DateTime PredictedAt { get; set; }
        public decimal Confidence { get; set; }
        public string Type { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public List<string> Factors { get; set; } = new();
        public string Status { get; set; } = string.Empty;
        public DateTime? CompletedAt { get; set; }
        public decimal? ActualValue { get; set; }
        public decimal Accuracy { get; set; }
        
        public virtual DobackAnalysis Analysis { get; set; } = null!;
    }
} 