using System;
using System.Collections.Generic;

namespace IncliSafe.Shared.Models.Analysis.Core
{
    public class PatternDetails
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<double> DetectedValues { get; set; } = new();
        public DateTime FirstDetection { get; set; }
        public int OccurrenceCount { get; set; }
        public decimal Confidence { get; set; }
    }
} 