using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Patterns;

namespace IncliSafe.Shared.Models.Analysis
{
    public class DobackAnalysis
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public DateTime Timestamp { get; set; }
        public string FileName { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime AnalysisDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal Confidence { get; set; }
        public decimal StabilityIndex { get; set; }
        
        // Relaciones
        public virtual ICollection<DobackData> Data { get; set; } = new List<DobackData>();
        public virtual ICollection<DetectedPattern> DetectedPatterns { get; set; } = new List<DetectedPattern>();
        public virtual ICollection<Prediction> Predictions { get; set; } = new List<Prediction>();
        
        // Relación uno a uno con AnalysisResult
        public virtual AnalysisResult? Result { get; set; }
    }
} 