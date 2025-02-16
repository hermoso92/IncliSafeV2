using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Notifications;

namespace IncliSafe.Shared.Models.Patterns
{
    public class PatternDetails
    {
        public int Id { get; set; }
        public string PatternName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public double Threshold { get; set; }
        public string Parameters { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public string ValidationRules { get; set; } = string.Empty;
        public virtual ICollection<DetectedPattern> Detections { get; set; } = new List<DetectedPattern>();
    }
} 