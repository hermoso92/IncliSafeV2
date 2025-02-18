using System;
using System.Collections.Generic;

namespace IncliSafe.Shared.Models.Analysis
{
    public class Prediction
    {
        public int Id { get; set; }
        public int AnalysisId { get; set; }
        public int VehicleId { get; set; }
        public PredictionType Type { get; set; }
        public decimal Value { get; set; }
        public decimal Confidence { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string> Recommendations { get; set; } = new();
        
        public virtual DobackAnalysis Analysis { get; set; } = null!;
        public virtual Vehiculo Vehicle { get; set; } = null!;
    }

    public enum PredictionType
    {
        Stability,
        Safety,
        Maintenance,
        Performance,
        Anomaly
    }

    public enum PredictionRisk
    {
        Low,
        Medium,
        High,
        Critical
    }
} 