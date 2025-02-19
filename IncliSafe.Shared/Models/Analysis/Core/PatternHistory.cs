using System;

namespace IncliSafe.Shared.Models.Analysis.Core
{
    public class PatternHistory
    {
        public int Id { get; set; }
        public int PatternId { get; set; }
        public DateTime DetectedAt { get; set; }
        public decimal Confidence { get; set; }
        public string Description { get; set; } = string.Empty;
    }
} 