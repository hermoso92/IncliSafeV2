using System;

namespace IncliSafe.Shared.Models.Patterns
{
    public class DetectionHistory
    {
        public int Id { get; set; }
        public int PatternId { get; set; }
        public DateTime DetectedAt { get; set; }
        public decimal Confidence { get; set; }
        public string Context { get; set; } = string.Empty;
        public string Values { get; set; } = string.Empty;
        public int VehicleId { get; set; }
        public virtual PatternDetails? Pattern { get; set; }
    }
} 