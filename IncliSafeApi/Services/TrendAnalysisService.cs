using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using IncliSafeApi.Data;
using IncliSafeApi.Services.Interfaces;
using IncliSafe.Shared.Models.DTOs;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Analysis.Core;
using IncliSafe.Shared.Models.Analysis.DTOs;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafeApi.Services
{
    public class TrendAnalysisService : ITrendAnalysisService
    {
        private readonly ApplicationDbContext _context;
        private readonly IVehicleMetricsService _metricsService;
        private readonly IAlertGenerationService _alertService;
        private readonly ILogger<TrendAnalysisService> _logger;

        public TrendAnalysisService(
            ApplicationDbContext context,
            IVehicleMetricsService metricsService,
            IAlertGenerationService alertService,
            ILogger<TrendAnalysisService> logger)
        {
            _context = context;
            _metricsService = metricsService;
            _alertService = alertService;
            _logger = logger;
        }

        public async Task<IEnumerable<TrendAnalysis>> AnalyzeTrendsAsync(int vehicleId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var historicalData = await GetHistoricalData(vehicleId, startDate, endDate);
                var patterns = await DetectPatterns(vehicleId);
                var predictions = await GeneratePredictions(vehicleId);

                var trendAnalysis = new List<TrendAnalysis>();
                trendAnalysis.AddRange(patterns);
                trendAnalysis.AddRange(predictions);

                return trendAnalysis;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al analizar tendencias para el vehículo {VehicleId}", vehicleId);
                return new List<TrendAnalysis>();
            }
        }

        private async Task<IEnumerable<AnalysisData>> GetHistoricalData(int vehicleId, DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _context.AnalysisData
                    .Where(d => d.VehicleId == vehicleId && 
                               d.AnalysisDate >= startDate && 
                               d.AnalysisDate <= endDate)
                    .OrderBy(d => d.AnalysisDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener datos históricos para el vehículo {VehicleId}", vehicleId);
                return new List<AnalysisData>();
            }
        }

        private async Task<IEnumerable<TrendAnalysis>> DetectPatterns(int vehicleId)
        {
            try
            {
                var patterns = await _context.AnalysisPatterns
                    .Where(p => p.VehicleId == vehicleId)
                    .OrderByDescending(p => p.DetectedAt)
                    .ToListAsync();

                return patterns.Select(p => new TrendAnalysis
                {
                    VehicleId = vehicleId,
                    AnalysisDate = p.DetectedAt,
                    PatternType = p.PatternType,
                    Description = p.Description
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al detectar patrones para el vehículo {VehicleId}", vehicleId);
                return new List<TrendAnalysis>();
            }
        }

        private async Task<IEnumerable<TrendAnalysis>> GeneratePredictions(int vehicleId)
        {
            try
            {
                var predictions = await _context.AnalysisPredictions
                    .Where(p => p.VehicleId == vehicleId)
                    .OrderByDescending(p => p.PredictionDate)
                    .ToListAsync();

                return predictions.Select(p => new TrendAnalysis
                {
                    VehicleId = vehicleId,
                    AnalysisDate = p.PredictionDate,
                    PatternType = "Predicción",
                    Description = p.Description
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar predicciones para el vehículo {VehicleId}", vehicleId);
                return new List<TrendAnalysis>();
            }
        }

        public async Task<TrendAnalysisDTO> GetTrendAnalysisAsync(int vehicleId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var trends = await AnalyzeTrendsAsync(vehicleId, startDate, endDate);
                var historicalData = await GetHistoricalData(vehicleId, startDate, endDate);

                return new TrendAnalysisDTO
                {
                    VehicleId = vehicleId,
                    StartDate = startDate,
                    EndDate = endDate,
                    Trends = trends.ToList(),
                    HistoricalData = historicalData.ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener análisis de tendencias para el vehículo {VehicleId}", vehicleId);
                return new TrendAnalysisDTO
                {
                    VehicleId = vehicleId,
                    StartDate = startDate,
                    EndDate = endDate,
                    Trends = new List<TrendAnalysis>(),
                    HistoricalData = new List<AnalysisData>()
                };
            }
        }

        public async Task<TrendAnalysisDTO> AnalyzeVehicleTrendsAsync(int vehicleId)
        {
            try
            {
                var analyses = await _context.DobackAnalyses
                    .Where(a => a.VehicleId == vehicleId)
                    .OrderByDescending(a => a.Timestamp)
                    .Take(30)
                    .ToListAsync();

                if (!analyses.Any())
                    return new TrendAnalysisDTO { VehicleId = vehicleId };

                var metrics = await _metricsService.GetVehicleMetricsAsync(vehicleId);
                var trendAnalysis = new TrendAnalysisDTO
                {
                    VehicleId = vehicleId,
                    StabilityTrend = CalculateStabilityTrend(analyses),
                    SafetyTrend = CalculateSafetyTrend(analyses),
                    PerformanceTrend = metrics.PerformanceTrend,
                    LastAnalysisDate = analyses.First().Timestamp,
                    TotalAnalyses = analyses.Count,
                    Recommendations = GenerateRecommendations(analyses)
                };

                await SaveTrendAnalysis(trendAnalysis);
                await GenerateAlertsFromTrends(trendAnalysis);

                return trendAnalysis;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error analyzing trends for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }

        private TrendDirection CalculateStabilityTrend(List<DobackAnalysis> analyses)
        {
            var recentAvg = analyses.Take(10).Average(a => a.StabilityScore);
            var olderAvg = analyses.Skip(10).Take(10).Average(a => a.StabilityScore);
            
            return CalculateTrendDirection(recentAvg, olderAvg);
        }

        private TrendDirection CalculateSafetyTrend(List<DobackAnalysis> analyses)
        {
            var recentAvg = analyses.Take(10).Average(a => a.SafetyScore);
            var olderAvg = analyses.Skip(10).Take(10).Average(a => a.SafetyScore);
            
            return CalculateTrendDirection(recentAvg, olderAvg);
        }

        private TrendDirection CalculateTrendDirection(decimal recent, decimal older)
        {
            var difference = recent - older;
            return difference switch
            {
                > 0.05M => TrendDirection.Improving,
                < -0.05M => TrendDirection.Declining,
                _ => TrendDirection.Stable
            };
        }

        private List<string> GenerateRecommendations(List<DobackAnalysis> analyses)
        {
            var recommendations = new List<string>();
            var latestAnalysis = analyses.First();

            if (latestAnalysis.StabilityScore < 0.7M)
            {
                recommendations.Add("Se recomienda una revisión del sistema de estabilidad.");
            }

            if (latestAnalysis.SafetyScore < 0.7M)
            {
                recommendations.Add("Se requiere una inspección de seguridad prioritaria.");
            }

            if (analyses.Count >= 3)
            {
                var recentScores = analyses.Take(3);
                if (recentScores.All(a => a.StabilityScore < 0.8M))
                {
                    recommendations.Add("Patrón de estabilidad subóptima detectado. Considerar mantenimiento preventivo.");
                }
            }

            return recommendations;
        }

        private async Task SaveTrendAnalysis(TrendAnalysisDTO analysis)
        {
            var trendAnalysis = new TrendAnalysis
            {
                VehicleId = analysis.VehicleId,
                StabilityTrend = analysis.StabilityTrend,
                SafetyTrend = analysis.SafetyTrend,
                AnalysisDate = DateTime.UtcNow,
                Recommendations = string.Join("|", analysis.Recommendations)
            };

            _context.TrendAnalysis.Add(trendAnalysis);
            await _context.SaveChangesAsync();
        }

        private async Task GenerateAlertsFromTrends(TrendAnalysisDTO analysis)
        {
            if (analysis.StabilityTrend == TrendDirection.Declining)
            {
                await _alertService.GenerateSafetyAlertAsync(analysis.VehicleId, 0.6M);
            }

            if (analysis.SafetyTrend == TrendDirection.Declining)
            {
                var alert = new VehicleAlertDTO
                {
                    VehicleId = analysis.VehicleId,
                    Title = "Tendencia de Seguridad Negativa",
                    Message = "Se ha detectado una tendencia negativa en los índices de seguridad.",
                    Type = AlertType.Safety,
                    Severity = AlertSeverity.Warning
                };

                await _alertService.CreateAlertAsync(analysis.VehicleId, alert);
            }
        }
    }
} 