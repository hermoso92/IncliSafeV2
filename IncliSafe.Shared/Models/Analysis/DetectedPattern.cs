using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Analysis.Core;

namespace IncliSafe.Shared.Models.Analysis
{
    public class DetectedPattern
    {
        public int Id { get; set; }
        public int PatternId { get; set; }
        public string PatternName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DateTime DetectionTime { get; set; }
        public DateTime FirstDetected { get; set; }
        public DateTime LastDetected { get; set; }
        public int TimesDetected { get; set; }
        public double ConfidenceScore { get; set; }
        public List<double> DetectedValues { get; set; } = new();
        public List<string> RecommendedActions { get; set; } = new();
        public int VehicleId { get; set; }
        public int? DobackAnalysisId { get; set; }
        public virtual DobackAnalysis Analysis { get; set; } = null!;
    }
} 