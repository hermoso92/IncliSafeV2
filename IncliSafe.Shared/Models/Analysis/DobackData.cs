using System;
using System.Collections.Generic;

namespace IncliSafe.Shared.Models.Analysis
{
    public class DobackData
    {
        public int Id { get; set; }
        public int CycleId { get; set; }
        public double AccelerometerX { get; set; }
        public double AccelerometerY { get; set; }
        public double AccelerometerZ { get; set; }
        public double Roll { get; set; }
        public double Pitch { get; set; }
        public double Yaw { get; set; }
        public double Speed { get; set; }
        public decimal StabilityIndex { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double TimeAntWifi { get; set; }
        public double USCycle1 { get; set; }
        public double USCycle2 { get; set; }
        public double USCycle3 { get; set; }
        public double USCycle4 { get; set; }
        public double USCycle5 { get; set; }
        public double AccMagnitude { get; set; }
        public double MicrosCleanCAN { get; set; }
        public double MicrosSD { get; set; }
        public int ErrorsCAN { get; set; }
        public double Steer { get; set; }
        public DateTime Timestamp { get; set; }
        public double Value { get; set; }
        public string SensorType { get; set; } = string.Empty;
        
        public virtual Cycle? Cycle { get; set; }
        
        // Análisis
        public bool RequiresAttention { get; set; }
        public decimal SafetyScore { get; set; }
        public decimal MaintenanceScore { get; set; }

        public int? DobackAnalysisId { get; set; }
        public virtual DobackAnalysis? Analysis { get; set; }
    }
}