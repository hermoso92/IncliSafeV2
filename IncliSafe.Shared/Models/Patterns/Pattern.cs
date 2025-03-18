using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;

namespace IncliSafe.Shared.Models.Patterns
{
    public class Pattern
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required PatternType Type { get; set; }
        public required decimal Confidence { get; set; }
        public required AlertSeverity Severity { get; set; }
        public required string Category { get; set; }
        public required string Source { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUsed { get; set; }
        public bool IsActive { get; set; }
        public List<string> Keywords { get; set; } = new();
        public List<string> Tags { get; set; } = new();
        public List<decimal> DataPoints { get; set; } = new();
        public List<DateTime> Timestamps { get; set; } = new();
        public Dictionary<string, object> Parameters { get; set; } = new();
        public Dictionary<string, object> Metadata { get; set; } = new();
        public List<PatternHistory> History { get; set; } = new();
    }
} 