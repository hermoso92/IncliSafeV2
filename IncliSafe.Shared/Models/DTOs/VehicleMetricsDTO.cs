using System;

namespace IncliSafe.Shared.Models.DTOs
{
    public class VehicleMetricsDTO
    {
        public int VehicleId { get; set; }
        public int TotalAnalyses { get; set; }
        public DateTime? LastAnalysisDate { get; set; }
        public decimal AverageStabilityScore { get; set; }
        public decimal AverageSafetyScore { get; set; }
        public PerformanceTrend PerformanceTrend { get; set; }
    }

    public enum PerformanceTrend
    {
        Improving,
        Stable,
        Declining
    }
} 