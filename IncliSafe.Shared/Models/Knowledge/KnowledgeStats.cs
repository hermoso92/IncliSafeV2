using System;
using System.Collections.Generic;

namespace IncliSafe.Shared.Models.Knowledge
{
    public class KnowledgeStats
    {
        public int TotalPatterns { get; set; }
        public int TotalDetections { get; set; }
        public double AverageConfidence { get; set; }
        public DateTime LastUpdate { get; set; }
        public int ActivePatterns { get; set; }
        public int InactivePatterns { get; set; }
        public Dictionary<string, int> PatternsByCategory { get; set; } = new();
    }
} 