using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Patterns;

namespace IncliSafe.Shared.Models.Patterns
{
    public class PatternDetection
    {
        public int Id { get; set; }
        public string PatternName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal ConfidenceScore { get; set; }
        public List<string> DetectedValues { get; set; } = new();
        public DateTime DetectionTime { get; set; }
        public int VehicleId { get; set; }
        public bool RequiresAction { get; set; }
        public string? ActionDescription { get; set; }
        
        public int KnowledgePatternId { get; set; }
        public virtual KnowledgePattern? KnowledgePattern { get; set; }
    }
} 