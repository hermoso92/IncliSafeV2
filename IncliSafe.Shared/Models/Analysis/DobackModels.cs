using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafe.Shared.Models.Analysis
{
    public class DobackAnalysis
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string FileName { get; set; } = string.Empty;
        public decimal StabilityIndex { get; set; }
        public decimal SafetyIndex { get; set; }
        public decimal Confidence { get; set; }
        public DateTime Timestamp { get; set; }
        public AnalysisResultType ResultType { get; set; }
        public virtual AnalysisResult Result { get; set; } = null!;
        public virtual ICollection<DobackData> Data { get; set; } = new List<DobackData>();
        public virtual ICollection<DetectedPattern> DetectedPatterns { get; set; } = new List<DetectedPattern>();
        public virtual ICollection<Prediction> Predictions { get; set; } = new List<Prediction>();
        public virtual Vehiculo? Vehicle { get; set; }
    }

    public class DobackData
    {
        public int Id { get; set; }
        public int DobackAnalysisId { get; set; }
        public decimal AccelerationX { get; set; }
        public decimal AccelerationY { get; set; }
        public decimal AccelerationZ { get; set; }
        public decimal Roll { get; set; }
        public decimal Pitch { get; set; }
        public decimal Yaw { get; set; }
        public decimal Speed { get; set; }
        public decimal StabilityIndex { get; set; }
        public decimal Temperature { get; set; }
        public decimal Humidity { get; set; }
        public decimal TimeAntWifi { get; set; }
        public decimal USCycle1 { get; set; }
        public decimal USCycle2 { get; set; }
        public decimal USCycle3 { get; set; }
        public decimal USCycle4 { get; set; }
        public decimal USCycle5 { get; set; }
        public decimal MicrosCleanCAN { get; set; }
        public decimal MicrosSD { get; set; }
        public decimal Steer { get; set; }
        public decimal SafetyScore { get; set; }
        public decimal MaintenanceScore { get; set; }
        public int CycleId { get; set; }
        public virtual DobackAnalysis Analysis { get; set; } = null!;
    }

    public class AnalysisResult
    {
        public int Id { get; set; }
        public int DobackAnalysisId { get; set; }
        public decimal EfficiencyScore { get; set; }
        public decimal MaintenanceScore { get; set; }
        public virtual DobackAnalysis DobackAnalysis { get; set; } = null!;
    }

    public enum AnalysisResultType
    {
        Normal,
        Warning,
        Critical,
        Error
    }
}
