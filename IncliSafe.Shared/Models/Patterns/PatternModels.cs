using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Patterns
{
    public class Pattern : BaseEntity
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required IncliSafe.Shared.Models.Enums.PatternType Type { get; set; }
        public required string PatternDefinition { get; set; }
        public required string Condition { get; set; }
        public required string Action { get; set; }
        public required int Priority { get; set; }
        public required bool IsActive { get; set; }
        public required IncliSafe.Shared.Models.Enums.AlertSeverity Severity { get; set; }
        public required int VehicleId { get; set; }
        public required DateTime DetectedAt { get; set; }
        public required decimal Confidence { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = new();
        public List<decimal> DataPoints { get; set; } = new();
        public List<DateTime> Timestamps { get; set; } = new();
        public Dictionary<string, object> Metadata { get; set; } = new();
        public virtual ICollection<PatternDetection> PatternDetections { get; set; } = new List<PatternDetection>();
    }

    public class PatternDetection : BaseEntity
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required IncliSafe.Shared.Models.Enums.PatternType Type { get; set; }
        public required string PatternDefinition { get; set; }
        public required string Condition { get; set; }
        public required string Action { get; set; }
        public required int Priority { get; set; }
        public required bool IsActive { get; set; }
        public required IncliSafe.Shared.Models.Enums.AlertSeverity Severity { get; set; }
        public required int VehicleId { get; set; }
        public required DateTime DetectedAt { get; set; }
        public required decimal Confidence { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = new();
        public List<decimal> DataPoints { get; set; } = new();
        public List<DateTime> Timestamps { get; set; } = new();
        public Dictionary<string, object> Metadata { get; set; } = new();
        public virtual Pattern Pattern { get; set; } = null!;
    }
} 