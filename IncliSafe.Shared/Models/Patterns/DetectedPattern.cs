using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Analysis;

namespace IncliSafe.Shared.Models.Patterns
{
    public class DetectedPattern
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Confidence { get; set; }
        public DateTime DetectedAt { get; set; }
        public string Status { get; set; } = string.Empty;
        public int AnalysisId { get; set; }
        public virtual DobackAnalysis Analysis { get; set; } = null!;
    }
} 