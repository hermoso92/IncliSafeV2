using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Patterns
{
    public class KnowledgePattern : BaseEntity
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required PatternType Type { get; set; }
        public required string Pattern { get; set; }
        public required string Condition { get; set; }
        public required string Action { get; set; }
        public required int Priority { get; set; }
        public required bool IsActive { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = new();
        public required AlertSeverity Severity { get; set; }
        public required decimal Confidence { get; set; }
        public required DateTime DetectedAt { get; set; }
        public List<string> Recommendations { get; set; } = new();
        public required string Category { get; set; }
        public required string Source { get; set; }
        public DateTime? LastUsed { get; set; }
        public List<string> Keywords { get; set; } = new();
        public List<string> Tags { get; set; } = new();
        public Dictionary<string, object> Metadata { get; set; } = new();
    }
} 