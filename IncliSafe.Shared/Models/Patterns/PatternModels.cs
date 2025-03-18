using IncliSafe.Shared.Models.Enums;
using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafe.Shared.Models.Patterns
{
    public class Pattern
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required PatternType Type { get; set; }
        public required decimal Confidence { get; set; }
        public required DateTime DetectedAt { get; set; }
        public required bool IsActive { get; set; }
        public List<PatternDataPoint> DataPoints { get; set; } = new();
        public Dictionary<string, string> Parameters { get; set; } = new();
        public Dictionary<string, string> Metadata { get; set; } = new();
    }

    public class PatternHistory
    {
        public required int Id { get; set; }
        public required int PatternId { get; set; }
        public required DateTime Timestamp { get; set; }
        public required string Event { get; set; }
        public required string Description { get; set; }
        public Dictionary<string, string> Metadata { get; set; } = new();
    }

    public class PatternDetection
    {
        public required int Id { get; set; }
        public required int VehicleId { get; set; }
        public required DateTime DetectedAt { get; set; }
        public required PatternType Type { get; set; }
        public required decimal Confidence { get; set; }
        public required string Description { get; set; }
        public List<PatternDataPoint> DataPoints { get; set; } = new();
        public Dictionary<string, string> Parameters { get; set; } = new();
        public Dictionary<string, string> Metadata { get; set; } = new();
        public virtual Vehicle Vehicle { get; set; } = null!;
    }

    public class KnowledgePattern
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required PatternType Type { get; set; }
        public required decimal Confidence { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required bool IsActive { get; set; }
        public List<PatternDataPoint> DataPoints { get; set; } = new();
        public Dictionary<string, string> Parameters { get; set; } = new();
        public Dictionary<string, string> Metadata { get; set; } = new();
    }
} 



