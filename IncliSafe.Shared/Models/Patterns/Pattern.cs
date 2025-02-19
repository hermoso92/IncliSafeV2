using System;
using System.Collections.Generic;

namespace IncliSafe.Shared.Models.Patterns
{
    public class Pattern
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public PatternType Type { get; set; }
        public decimal Confidence { get; set; }
        public DateTime DetectedAt { get; set; }
        public int VehicleId { get; set; }
        public string Category { get; set; } = string.Empty;
        public decimal Severity { get; set; }
        public bool IsActive { get; set; }
        public virtual Vehiculo Vehicle { get; set; } = null!;
    }

    public enum PatternType
    {
        Stability,
        Safety,
        Maintenance,
        Performance,
        Anomaly
    }
} 