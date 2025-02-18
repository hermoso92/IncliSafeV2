using IncliSafe.Shared.Models.DTOs;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafeApi.Services.Mapping
{
    public static class VehicleMetricsMapper
    {
        public static VehicleMetricsDTO ToDTO(this VehicleMetrics metrics, PerformanceTrend trend)
        {
            return new VehicleMetricsDTO
            {
                VehicleId = metrics.VehicleId,
                TotalAnalyses = metrics.TotalAnalyses,
                LastAnalysisDate = metrics.LastAnalysisDate,
                AverageStabilityScore = metrics.AverageStabilityScore,
                AverageSafetyScore = metrics.AverageSafetyScore,
                PerformanceTrend = trend
            };
        }
    }
} 