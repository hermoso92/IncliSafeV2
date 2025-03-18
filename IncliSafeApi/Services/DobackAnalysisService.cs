using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using IncliSafeApi.Data;
using IncliSafeApi.Services.Interfaces;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Shared.Exceptions;
using IncliSafe.Shared.Models.Analysis.Core;
using IncliSafe.Shared.Models.DTOs;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafeApi.Services
{
    public class DobackAnalysisService : IDobackAnalysisService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DobackAnalysisService> _logger;

        public DobackAnalysisService(
            ApplicationDbContext context,
            ILogger<DobackAnalysisService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<DobackAnalysis>> GetAnalysesAsync(int vehicleId)
        {
            try
            {
                return await _context.DobackAnalyses
                    .Include(a => a.Data)
                    .Include(a => a.DetectedPatterns)
                    .Include(a => a.Predictions)
                    .Include(a => a.Anomalies)
                    .Where(a => a.VehicleId == vehicleId)
                    .OrderByDescending(a => a.Timestamp)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting analyses for vehicle {Id}", vehicleId);
                throw;
            }
        }

        public async Task<TrendAnalysisEntity> GetTrendAnalysis(int analysisId)
        {
            try
            {
                return await _context.TrendAnalyses
                    .Include(t => t.Predictions)
                    .Include(t => t.Anomalies)
                    .FirstOrDefaultAsync(t => t.DobackAnalysisId == analysisId)
                    ?? throw new ApiException("Análisis de tendencias no encontrado", System.Net.HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting trend analysis for analysis {Id}", analysisId);
                throw;
            }
        }

        public async Task<List<AnalysisPrediction>> GetPredictionsAsync(int analysisId)
        {
            try
            {
                return await _context.AnalysisPredictions
                    .Include(p => p.Vehicle)
                    .Include(p => p.Analysis)
                    .Where(p => p.AnalysisId == analysisId)
                    .OrderByDescending(p => p.PredictedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting predictions for analysis {Id}", analysisId);
                throw;
            }
        }

        public async Task<List<Anomaly>> GetAnomaliesAsync(int analysisId)
        {
            try
            {
                return await _context.Anomalies
                    .Include(a => a.Vehicle)
                    .Include(a => a.Analysis)
                    .Where(a => a.AnalysisId == analysisId)
                    .OrderByDescending(a => a.DetectedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting anomalies for analysis {Id}", analysisId);
                throw;
            }
        }

        public async Task<DobackAnalysis> CreateAnalysisAsync(DobackAnalysis analysis)
        {
            try
            {
                _context.DobackAnalyses.Add(analysis);
                await _context.SaveChangesAsync();
                return analysis;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating analysis for vehicle {Id}", analysis.VehicleId);
                throw;
            }
        }

        public async Task<bool> DeleteAnalysisAsync(int analysisId)
        {
            try
            {
                var analysis = await _context.DobackAnalyses.FindAsync(analysisId);
                if (analysis == null) return false;

                _context.DobackAnalyses.Remove(analysis);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting analysis {Id}", analysisId);
                throw;
            }
        }

        public async Task<List<DobackData>> GetDobackDataAsync(int analysisId)
        {
            try
            {
                return await _context.DobackData
                    .Where(d => d.DobackAnalysisId == analysisId)
                    .OrderBy(d => d.Timestamp)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting data for analysis {Id}", analysisId);
                throw;
            }
        }

        public async Task<AnalysisResult> GetAnalysisResultAsync(int analysisId)
        {
            try
            {
                var analysis = await _context.DobackAnalyses
                    .Include(a => a.Result)
                    .FirstOrDefaultAsync(a => a.Id == analysisId);

                return analysis?.Result ?? throw new ApiException("Resultado de análisis no encontrado", System.Net.HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting analysis result for {Id}", analysisId);
                throw;
            }
        }

        public async Task<List<PatternDetails>> GetPatternsAsync(int analysisId)
        {
            try
            {
                return await _context.PatternDetails
                    .Include(p => p.DobackAnalysis)
                    .Where(p => p.DobackAnalysisId == analysisId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting patterns for analysis {Id}", analysisId);
                throw;
            }
        }

        public async Task<List<DobackData>> GetHistoricalData(int vehicleId, DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _context.DobackData
                    .Include(d => d.DobackAnalysis)
                    .Where(d => d.DobackAnalysis.VehicleId == vehicleId && d.Timestamp >= startDate && d.Timestamp <= endDate)
                    .OrderBy(d => d.Timestamp)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting historical data for vehicle {Id} from {StartDate} to {EndDate}", vehicleId, startDate, endDate);
                throw;
            }
        }

        public async Task<DashboardMetrics> GetDashboardMetricsAsync(int vehicleId)
        {
            try
            {
                var metrics = new DashboardMetrics
                {
                    VehicleId = vehicleId,
                    LastUpdated = DateTime.UtcNow
                };

                var recentAnalyses = await _context.DobackAnalyses
                    .Where(a => a.VehicleId == vehicleId)
                    .OrderByDescending(a => a.AnalysisDate)
                    .Take(10)
                    .ToListAsync();

                if (recentAnalyses.Any())
                {
                    metrics.StabilityScore = recentAnalyses.Average(a => a.StabilityScore);
                    metrics.SafetyScore = recentAnalyses.Average(a => a.SafetyScore);
                    metrics.MaintenanceScore = recentAnalyses.Average(a => a.MaintenanceScore);
                    metrics.LastAnalysisDate = recentAnalyses.First().AnalysisDate;
                }

                return metrics;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dashboard metrics for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<DobackAnalysis> GetAnalysisAsync(int analysisId)
        {
            try
            {
                var analysis = await _context.DobackAnalyses
                    .Include(a => a.Data)
                    .FirstOrDefaultAsync(a => a.Id == analysisId);

                if (analysis == null)
                {
                    throw new InvalidOperationException($"Analysis with ID {analysisId} not found.");
                }

                return analysis;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting analysis {AnalysisId}", analysisId);
                throw;
            }
        }

        public async Task<List<PatternDetails>> GetPatternsAsync(int vehicleId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var patterns = await _context.PatternDetails
                    .Where(p => p.VehicleId == vehicleId && p.DetectionDate >= startDate && p.DetectionDate <= endDate)
                    .OrderByDescending(p => p.DetectionDate)
                    .AsNoTracking()
                    .ToListAsync();

                return patterns;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting patterns for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<TrendAnalysis> GetTrendAnalysisAsync(int vehicleId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var analyses = await _context.DobackAnalyses
                    .Where(a => a.VehicleId == vehicleId && a.AnalysisDate >= startDate && a.AnalysisDate <= endDate)
                    .OrderBy(a => a.AnalysisDate)
                    .ToListAsync();

                var trendAnalysis = new TrendAnalysis
                {
                    VehicleId = vehicleId,
                    StartDate = startDate,
                    EndDate = endDate,
                    AnalysisCount = analyses.Count,
                    StabilityTrend = CalculateTrend(analyses.Select(a => a.StabilityScore).ToList()),
                    SafetyTrend = CalculateTrend(analyses.Select(a => a.SafetyScore).ToList()),
                    MaintenanceTrend = CalculateTrend(analyses.Select(a => a.MaintenanceScore).ToList())
                };

                return trendAnalysis;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting trend analysis for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<DobackAnalysis> CreateAnalysisAsync(int vehicleId, List<DobackData> data)
        {
            try
            {
                var analysis = new DobackAnalysis
                {
                    VehicleId = vehicleId,
                    AnalysisDate = DateTime.UtcNow,
                    Data = data,
                    StabilityScore = CalculateStabilityScore(data),
                    SafetyScore = CalculateSafetyScore(data),
                    MaintenanceScore = CalculateMaintenanceScore(data)
                };

                _context.DobackAnalyses.Add(analysis);
                await _context.SaveChangesAsync();

                return analysis;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating analysis for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<DobackAnalysis> AnalyzeDobackAsync(int vehicleId, DobackAnalysisDTO analysisDto)
        {
            try
            {
                var analysis = new DobackAnalysis
                {
                    VehicleId = vehicleId,
                    AnalysisDate = analysisDto.AnalysisDate,
                    Data = analysisDto.Data,
                    StabilityScore = analysisDto.StabilityScore,
                    SafetyScore = analysisDto.SafetyScore,
                    MaintenanceScore = analysisDto.MaintenanceScore,
                    Notes = analysisDto.Notes,
                    RequiresAttention = analysisDto.RequiresAttention,
                    Warnings = analysisDto.Warnings,
                    Recommendations = analysisDto.Recommendations
                };

                _context.DobackAnalyses.Add(analysis);
                await _context.SaveChangesAsync();

                return analysis;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error analyzing doback for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }

        private double CalculateTrend(List<double> values)
        {
            if (values == null || values.Count < 2)
                return 0;

            var n = values.Count;
            var xMean = (n - 1) / 2.0;
            var yMean = values.Average();

            var numerator = 0.0;
            var denominator = 0.0;

            for (int i = 0; i < n; i++)
            {
                var x = i - xMean;
                var y = values[i] - yMean;
                numerator += x * y;
                denominator += x * x;
            }

            return denominator == 0 ? 0 : numerator / denominator;
        }

        private double CalculateStabilityScore(List<DobackData> data)
        {
            // Implementar lógica para calcular el score de estabilidad
            return 0.0;
        }

        private double CalculateSafetyScore(List<DobackData> data)
        {
            // Implementar lógica para calcular el score de seguridad
            return 0.0;
        }

        private double CalculateMaintenanceScore(List<DobackData> data)
        {
            // Implementar lógica para calcular el score de mantenimiento
            return 0.0;
        }
    }
} 