using System;
using System.Collections.Generic;

namespace IncliSafe.Shared.Models.Patterns
{
    public class DetectedPattern
    {
        public int Id { get; set; }
        public string PatternName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal ConfidenceScore { get; set; }
        public List<string> DetectedValues { get; set; } = new();
        public DateTime DetectionTime { get; set; }
        public int VehicleId { get; set; }
        public List<string> RecommendedActions { get; set; } = new();
        
        // Agregar la relación con DobackAnalysis
        public int? DobackAnalysisId { get; set; }
        public virtual DobackAnalysis? DobackAnalysis { get; set; }
    }
} 