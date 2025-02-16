using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Patterns;

namespace IncliSafe.Shared.Models.Knowledge
{
    public class KnowledgePattern
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Pattern { get; set; } = string.Empty;
        public List<string> Actions { get; set; } = new();
        public List<string> RecommendedActions { get; set; } = new();
        public bool IsActive { get; set; } = true;
        public double Confidence { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModified { get; set; }
        public virtual ICollection<PatternDetection> Detections { get; set; } = new List<PatternDetection>();
    }
} 