using System;
using System.Collections.Generic;
using System.Linq;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Patterns;
using IncliSafe.Shared.Models.Analysis;  // Para AnalysisResult

namespace IncliSafe.Shared.Models.Analysis.Core
{
    public class DobackAnalysis
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = string.Empty;
        public decimal StabilityScore { get; set; }
        public decimal SafetyScore { get; set; }
        public decimal MaintenanceScore { get; set; }
        public string FileName { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string FileType { get; set; } = string.Empty;
        public AnalysisResult Result { get; set; } = new();
        public List<string> Warnings { get; set; } = new();
        public List<string> Errors { get; set; } = new();
        public decimal StabilityIndex { get; set; }
        public decimal Confidence { get; set; }
        
        public virtual Vehiculo Vehicle { get; set; } = null!;
        public virtual ICollection<DobackData> Data { get; set; } = new List<DobackData>();
        public virtual ICollection<DetectedPattern> DetectedPatterns { get; set; } = new List<DetectedPattern>();
        public virtual ICollection<Prediction> Predictions { get; set; } = new List<Prediction>();
        public virtual ICollection<Anomaly> Anomalies { get; set; } = new List<Anomaly>();
        public virtual TrendAnalysis TrendAnalysis { get; set; } = null!;

        public string GetStatusDisplay() => Status switch
        {
            "Processing" => "Procesando",
            "Completed" => "Completado",
            "Failed" => "Fallido",
            "Pending" => "Pendiente",
            _ => Status
        };

        public decimal GetOverallScore() =>
            (StabilityScore + SafetyScore + MaintenanceScore) / 3;

        public bool HasWarnings() => Warnings.Count > 0;
        public bool HasErrors() => Errors.Count > 0;
        public bool IsComplete() => Status == "Completed";

        public ICollection<DobackData> GetData() => Data;
        public decimal GetStabilityScore() => StabilityScore;
        public decimal GetSafetyScore() => SafetyScore;
        public decimal GetMaintenanceScore() => MaintenanceScore;
        public decimal GetSpeed() => Data.Average(d => d.Speed);
        public decimal GetRoll() => Data.Average(d => d.Roll);
        public decimal GetPitch() => Data.Average(d => d.Pitch);
        public decimal GetYaw() => Data.Average(d => d.Yaw);
    }
} 