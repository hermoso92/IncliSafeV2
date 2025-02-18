using System;

namespace IncliSafe.Shared.Models.Analysis
{
    public class DobackData
    {
        public int Id { get; set; }
        public int AnalysisId { get; set; }
        public DateTime Timestamp { get; set; }
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
        
        public virtual DobackAnalysis Analysis { get; set; } = null!;
    }
} 