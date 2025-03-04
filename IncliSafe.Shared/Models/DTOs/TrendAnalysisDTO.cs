using System;
using System.Collections.Generic;

namespace IncliSafe.Shared.Models.DTOs
{
    public class TrendAnalysisDTO
    {
        public int VehicleId { get; set; }
        public TrendDirection StabilityTrend { get; set; }
        public TrendDirection SafetyTrend { get; set; }
        public PerformanceTrend PerformanceTrend { get; set; }
        public DateTime? LastAnalysisDate { get; set; }
        public int TotalAnalyses { get; set; }
        public List<string> Recommendations { get; set; } = new();
    }

    public enum TrendDirection
    {
        Improving,
        Stable,
        Declining
    }
} 