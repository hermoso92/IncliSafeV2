using IncliSafe.Shared.Models.Enums;
using System;
using System.Collections.Generic;

namespace IncliSafe.Shared.Models.Patterns
{
    public class DetectedPattern
    {
        public required int Id { get; set; }
        public required int VehicleId { get; set; }
        public required DateTime DetectionDate { get; set; }
        public required string PatternType { get; set; }
        public required decimal Confidence { get; set; }
        public required string Description { get; set; }
        public List<PatternDataPoint> DataPoints { get; set; } = new();
        public Dictionary<string, string> Metadata { get; set; } = new();
    }

    public class PatternDataPoint
    {
        public required DateTime Timestamp { get; set; }
        public required decimal Value { get; set; }
        public required string Label { get; set; }
        public required bool IsAnomaly { get; set; }
    }

    public class PatternMatch
    {
        public required int Id { get; set; }
        public required int PatternId { get; set; }
        public required decimal MatchScore { get; set; }
        public required DateTime MatchDate { get; set; }
        public List<string> MatchedFeatures { get; set; } = new();
    }
} 

