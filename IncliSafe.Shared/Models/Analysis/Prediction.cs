using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Analysis.Core;

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
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime PredictedFor { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal Accuracy { get; set; }
        public decimal? ActualValue { get; set; }
        public string Category { get; set; } = string.Empty;
        public List<string> Recommendations { get; set; } = new();
        
        public virtual DobackAnalysis Analysis { get; set; } = null!;
        public virtual Vehiculo Vehicle { get; set; } = null!;

        public string GetStatusDisplay() => Status switch
        {
            "Pending" => "Pendiente",
            "Confirmed" => "Confirmado",
            "Invalid" => "Inválido",
            _ => Status
        };

        public string GetAccuracyLevel() => Accuracy switch
        {
            > 0.9M => "Alta",
            > 0.7M => "Media",
            > 0.5M => "Baja",
            _ => "Muy Baja"
        };
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