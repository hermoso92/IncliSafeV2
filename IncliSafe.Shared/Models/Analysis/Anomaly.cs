using System;
using System.Collections.Generic;

namespace IncliSafe.Shared.Models.Analysis
{
    public class Anomaly
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public AnomalyType Type { get; set; }
        public double Value { get; set; }
        public double Threshold { get; set; }
        public double Severity { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsResolved { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public string Resolution { get; set; } = string.Empty;
        public int CycleId { get; set; }
        public virtual Cycle? Cycle { get; set; }
        public int? TrendAnalysisId { get; set; }
        public virtual TrendAnalysis? TrendAnalysis { get; set; }
    }
}