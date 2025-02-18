using System;
using System.Collections.Generic;
using System.Linq;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Patterns;

namespace IncliSafe.Shared.Models.Analysis
{
    public class DobackAnalysis
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal StabilityScore { get; set; }
        public decimal SafetyScore { get; set; }
        public decimal MaintenanceScore { get; set; }
        public decimal Confidence { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        public virtual Vehiculo Vehicle { get; set; } = null!;
        public virtual ICollection<DobackData> Data { get; set; } = new List<DobackData>();
        public virtual ICollection<DetectedPattern> DetectedPatterns { get; set; } = new List<DetectedPattern>();
        public virtual ICollection<AnalysisPrediction> Predictions { get; set; } = new List<AnalysisPrediction>();
        public virtual AnalysisResult Result { get; set; } = new();

        public ICollection<DobackData> GetData() => Data;
        public decimal GetStabilityScore() => Result.StabilityScore;
        public decimal GetSafetyScore() => Result.SafetyScore;
        public decimal GetSpeed() => Data.Average(d => d.Speed);
        public decimal GetRoll() => Data.Average(d => d.Roll);
        public decimal GetPitch() => Data.Average(d => d.Pitch);
        public decimal GetYaw() => Data.Average(d => d.Yaw);
    }
} 