using IncliSafe.Shared.Models.Enums;
using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Analysis;

namespace IncliSafe.Shared.Models.Patterns
{
    public class DetectionHistory
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal Value { get; set; }
        public required DateTime DetectedAt { get; set; }
        public required bool IsActive { get; set; }
        public List<DetectedPattern> Patterns { get; set; } = new();
    }
} 

