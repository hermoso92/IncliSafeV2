using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Patterns
{
    public class PatternDetection : BaseEntity
    {
        public override Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required PatternType Type { get; set; }
        public required AlertSeverity Severity { get; set; }
        public required Guid VehicleId { get; set; }
        public required DateTime DetectedAt { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = new();
        public override DateTime CreatedAt { get; set; }
        public required decimal Confidence { get; set; }
        public List<decimal> DataPoints { get; set; } = new();
        public List<DateTime> Timestamps { get; set; } = new();
        public Dictionary<string, object> Metadata { get; set; } = new();
        public virtual Pattern Pattern { get; set; } = null!;
    }
} 