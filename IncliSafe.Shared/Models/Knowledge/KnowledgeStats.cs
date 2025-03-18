using IncliSafe.Shared.Models.Enums;
using System;
using System.Collections.Generic;

namespace IncliSafe.Shared.Models.Knowledge
{
    public class KnowledgeStats
    {
        public required int TotalPatterns { get; set; }
        public required int TotalDetections { get; set; }
        public required double AverageConfidence { get; set; }
        public required DateTime LastUpdate { get; set; }
        public required int ActivePatterns { get; set; }
        public required int InactivePatterns { get; set; }
        public Dictionary<string, int> PatternsByCategory { get; set; } = new();
    }
} 

