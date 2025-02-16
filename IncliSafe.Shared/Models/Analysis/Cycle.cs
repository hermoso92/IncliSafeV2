using System;
using System.Collections.Generic;

namespace IncliSafe.Shared.Models.Analysis
{
    public class Cycle
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public double Duration { get; set; }
        public double AverageSpeed { get; set; }
        public double MaxSpeed { get; set; }
        public decimal StabilityIndex { get; set; }
        public decimal SafetyScore { get; set; }
        public List<Anomaly> Anomalies { get; set; } = new();
        public int VehicleId { get; set; }
        public string Status { get; set; } = string.Empty;
        public virtual ICollection<DobackData> DobackData { get; set; } = new List<DobackData>();
    }
} 