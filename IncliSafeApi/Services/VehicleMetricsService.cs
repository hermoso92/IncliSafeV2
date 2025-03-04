using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using IncliSafeApi.Services.Interfaces;
using IncliSafe.Shared.Models.DTOs;
using IncliSafeApi.Data;

namespace IncliSafeApi.Services
{
    public class VehicleMetricsService : IVehicleMetricsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAlertGenerationService _alertService;
        private readonly ILogger<VehicleMetricsService> _logger;

        public VehicleMetricsService(
            ApplicationDbContext context,
            IAlertGenerationService alertService,
            ILogger<VehicleMetricsService> logger)
        {
            _context = context;
            _alertService = alertService;
            _logger = logger;
        }

        public async Task ProcessNewAnalysisAsync(int vehicleId, decimal stabilityScore, decimal safetyScore)
        {
            try
            {
                // Verificar umbrales de seguridad
                if (safetyScore < 0.7M)
                {
                    await _alertService.GenerateSafetyAlertAsync(vehicleId, safetyScore);
                }

                // Actualizar métricas históricas
                var metrics = await GetOrCreateMetrics(vehicleId);
                metrics.LastAnalysisDate = DateTime.UtcNow;
                metrics.TotalAnalyses++;
                metrics.AverageStabilityScore = ((metrics.AverageStabilityScore * (metrics.TotalAnalyses - 1)) + stabilityScore) / metrics.TotalAnalyses;
                metrics.AverageSafetyScore = ((metrics.AverageSafetyScore * (metrics.TotalAnalyses - 1)) + safetyScore) / metrics.TotalAnalyses;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing new analysis for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<VehicleMetricsDTO> GetVehicleMetricsAsync(int vehicleId)
        {
            try
            {
                var metrics = await GetOrCreateMetrics(vehicleId);
                return new VehicleMetricsDTO
                {
                    VehicleId = vehicleId,
                    TotalAnalyses = metrics.TotalAnalyses,
                    LastAnalysisDate = metrics.LastAnalysisDate,
                    AverageStabilityScore = metrics.AverageStabilityScore,
                    AverageSafetyScore = metrics.AverageSafetyScore,
                    PerformanceTrend = CalculatePerformanceTrend(metrics)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting metrics for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }

        private async Task<VehicleMetrics> GetOrCreateMetrics(int vehicleId)
        {
            var metrics = await _context.VehicleMetrics
                .FirstOrDefaultAsync(m => m.VehicleId == vehicleId);

            if (metrics == null)
            {
                metrics = new VehicleMetrics
                {
                    VehicleId = vehicleId,
                    TotalAnalyses = 0,
                    AverageStabilityScore = 0,
                    AverageSafetyScore = 0,
                    LastAnalysisDate = null
                };
                _context.VehicleMetrics.Add(metrics);
            }

            return metrics;
        }

        private PerformanceTrend CalculatePerformanceTrend(VehicleMetrics metrics)
        {
            if (!metrics.LastAnalysisDate.HasValue || metrics.TotalAnalyses < 2)
                return PerformanceTrend.Stable;

            var recentAnalyses = _context.DobackAnalyses
                .Where(a => a.VehicleId == metrics.VehicleId)
                .OrderByDescending(a => a.Timestamp)
                .Take(5)
                .ToList();

            if (recentAnalyses.Count < 2)
                return PerformanceTrend.Stable;

            var avgTrend = recentAnalyses.Average(a => a.StabilityScore);
            var diff = avgTrend - metrics.AverageStabilityScore;

            return diff switch
            {
                > 0.05M => PerformanceTrend.Improving,
                < -0.05M => PerformanceTrend.Declining,
                _ => PerformanceTrend.Stable
            };
        }

        public async Task UpdateMetricsAsync(int vehicleId)
        {
            try
            {
                var metrics = await GetOrCreateMetrics(vehicleId);
                var analyses = await _context.DobackAnalyses
                    .Where(a => a.VehicleId == vehicleId)
                    .OrderByDescending(a => a.Timestamp)
                    .Take(30)
                    .ToListAsync();

                if (analyses.Any())
                {
                    metrics.LastAnalysisDate = analyses.First().Timestamp;
                    metrics.TotalAnalyses = analyses.Count;
                    metrics.AverageStabilityScore = analyses.Average(a => a.StabilityIndex);
                    metrics.AverageSafetyScore = analyses.Average(a => a.SafetyScore);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating metrics for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<decimal> CalculateStabilityScoreAsync(int vehicleId)
        {
            try
            {
                var recentAnalyses = await _context.DobackAnalyses
                    .Where(a => a.VehicleId == vehicleId)
                    .OrderByDescending(a => a.Timestamp)
                    .Take(10)
                    .ToListAsync();

                if (!recentAnalyses.Any())
                    return 0;

                return recentAnalyses.Average(a => a.StabilityIndex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating stability score for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }
    }
} 