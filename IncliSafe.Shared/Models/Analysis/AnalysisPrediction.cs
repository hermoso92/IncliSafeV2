using System;

namespace IncliSafe.Shared.Models.Analysis
{
    public class AnalysisPrediction
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public double PredictedValue { get; set; }
        public double ActualValue { get; set; }
        public double Confidence { get; set; }
        public string Description { get; set; } = string.Empty;
        public PredictionRisk Risk { get; set; }
        public int VehicleId { get; set; }
        public int DobackAnalysisId { get; set; }
        public virtual Entities.Vehiculo Vehicle { get; set; } = null!;
        public virtual DobackAnalysis DobackAnalysis { get; set; } = null!;
        public string Type { get; set; } = string.Empty;
    }
} 