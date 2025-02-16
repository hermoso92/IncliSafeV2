using System;
using System.Collections.Generic;

namespace IncliSafe.Shared.Models.Patterns
{
    public class Pattern
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? LastDetected { get; set; }
        public int DetectionCount { get; set; }
        public List<DetectionHistory> History { get; set; } = new();
    }
} 