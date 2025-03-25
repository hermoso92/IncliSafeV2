using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IncliSafeApi.Data;
using IncliSafeApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Patterns;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Shared.Models.Analysis.Core;
using IncliSafe.Shared.Models.Notifications;
using IncliSafe.Shared.Models.DTOs;
using IncliSafe.Shared.Models.Enums;
using IncliSafe.Shared.DTOs;
using CoreAnalysisPrediction = IncliSafe.Shared.Models.Analysis.Core.AnalysisPrediction;
using IncliSafe.Shared.Models.Analysis.Core;
using IncliSafe.Shared.Models.Analysis.DTOs;

namespace IncliSafeApi.Services
{
    public class DobackService : IDobackService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DobackService> _logger;
        private readonly IDobackAnalysisService _analysisService;
        private readonly IPredictiveAnalysisService _predictiveService;

        public DobackService(
            ApplicationDbContext context,
            ILogger<DobackService> logger,
            IDobackAnalysisService analysisService,
            IPredictiveAnalysisService predictiveService)
        {
            _context = context;
            _logger = logger;
            _analysisService = analysisService;
            _predictiveService = predictiveService;
        }

        public async Task<DashboardMetrics> GetDashboardMetrics()
        {
            try
            {
                var metrics = await _context.DashboardMetrics.FirstOrDefaultAsync();
                return metrics ?? new DashboardMetrics();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener métricas del dashboard");
                return new DashboardMetrics();
            }
        }

        public async Task<DobackAnalysis> GetAnalysis(int vehicleId)
        {
            try
            {
                var analysis = await _context.Analyses
                    .Include(a => a.Patterns)
                    .Include(a => a.Predictions)
                    .FirstOrDefaultAsync(a => a.VehicleId == vehicleId);

                if (analysis == null)
                {
                    _logger.LogWarning("No se encontró análisis para el vehículo {VehicleId}", vehicleId);
                    return null;
                }

                return analysis;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener análisis para el vehículo {VehicleId}", vehicleId);
                return null;
            }
        }

        public async Task<IEnumerable<DobackAnalysis>> GetAnalyses()
        {
            try
            {
                return await _context.Analyses
                    .Include(a => a.Patterns)
                    .Include(a => a.Predictions)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener análisis");
                return new List<DobackAnalysis>();
            }
        }

        public async Task<IEnumerable<AnalysisPattern>> GetDetectedPatternsAsync(int vehicleId)
        {
            try
            {
                return await _context.Patterns
                    .Where(p => p.VehicleId == vehicleId && p.DetectedAt != null)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener patrones detectados para el vehículo {VehicleId}", vehicleId);
                return new List<AnalysisPattern>();
            }
        }

        public async Task<IEnumerable<TrendAnalysis>> GetTrendAnalysisAsync(int vehicleId, DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _context.TrendAnalyses
                    .Where(t => t.VehicleId == vehicleId && 
                               t.AnalysisDate >= startDate && 
                               t.AnalysisDate <= endDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener análisis de tendencias para el vehículo {VehicleId}", vehicleId);
                return new List<TrendAnalysis>();
            }
        }

        public async Task<AnalysisResult> GetAnalysisResult(int vehicleId)
        {
            try
            {
                var analysis = await _context.Analyses
                    .Where(a => a.VehicleId == vehicleId)
                    .OrderByDescending(a => a.AnalysisDate)
                    .FirstOrDefaultAsync();

                return analysis?.Result ?? AnalysisResult.Unknown;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener resultado de análisis para el vehículo {VehicleId}", vehicleId);
                return AnalysisResult.Unknown;
            }
        }

        public async Task<bool> DeleteAnalysisAsync(int id)
        {
            try
            {
                var analysis = await _context.DobackAnalyses.FindAsync(id);
                if (analysis == null) return false;
                
                _context.DobackAnalyses.Remove(analysis);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting analysis {AnalysisId}", id);
                return false;
            }
        }

        public async Task<List<DobackData>> GetDobackDataAsync(int analysisId)
        {
            try
            {
                var analysis = await _context.DobackAnalyses
                    .Include(a => a.Data)
                    .FirstOrDefaultAsync(a => a.Id == analysisId);

                return analysis?.Data.OrderBy(d => d.Timestamp).ToList() ?? new List<DobackData>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting doback data for analysis {AnalysisId}", analysisId);
                throw;
            }
        }

        public async Task<List<DobackData>> GetDobackData(int vehicleId, DateTime start, DateTime end)
        {
            try
            {
                return await _context.DobackData
                    .Where(d => d.VehicleId == vehicleId &&
                           d.Timestamp >= start &&
                           d.Timestamp <= end)
                    .OrderBy(d => d.Timestamp)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting doback data for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<List<Alert>> GetRecentAlertsAsync(int vehicleId, int count = 10)
        {
            try
            {
                return await _context.Alerts
                    .Where(a => a.VehicleId == vehicleId)
                    .OrderByDescending(a => a.CreatedAt)
                    .Take(count)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting recent alerts for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<List<AnalysisAnomaly>> GetAnomalies(int vehicleId)
        {
            try
            {
                return await _context.Anomalies
                    .Where(a => a.VehicleId == vehicleId)
                    .OrderByDescending(a => a.DetectedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting anomalies for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<List<AnalysisPattern>> GetPatterns(int vehicleId)
        {
            try
            {
                return await _context.Patterns
                    .Where(p => p.VehicleId == vehicleId)
                    .OrderByDescending(p => p.DetectedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting patterns for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<List<AnalysisPrediction>> GetPredictions(int vehicleId)
        {
            try
            {
                return await _context.AnalysisPredictions
                    .Where(p => p.VehicleId == vehicleId)
                    .OrderByDescending(p => p.GeneratedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting predictions for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<AnalysisPrediction> GeneratePredictionAsync(int vehicleId)
        {
            try
            {
                return await _predictiveService.GeneratePredictionAsync(vehicleId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating prediction for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<CoreMetrics> GetMetricsAsync()
        {
            try
            {
                return await _analysisService.GetMetricsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting metrics");
                throw;
            }
        }

        public async Task<DobackAnalysis> GetLatestAnalysisAsync(int vehicleId)
        {
            try
            {
                return await _context.DobackAnalyses
                    .Where(a => a.VehicleId == vehicleId)
                    .OrderByDescending(a => a.AnalysisDate)
                    .FirstOrDefaultAsync() ?? throw new InvalidOperationException("No analysis found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting latest analysis for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<DobackAnalysis?> GetLatestAnalysis(int vehicleId)
        {
            try
            {
                return await _context.DobackAnalyses
                    .Where(a => a.VehicleId == vehicleId)
                    .OrderByDescending(a => a.AnalysisDate)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting latest analysis for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<DobackAnalysis> AnalyzeDobackAsync(int vehicleId, DobackAnalysisDTO analysisDto)
        {
            try
            {
                return await _analysisService.AnalyzeDobackAsync(vehicleId, analysisDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error analyzing doback for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<DobackAnalysis> GetAnalysisByIdAsync(int analysisId)
        {
            try
            {
                return await _context.DobackAnalyses
                    .Include(a => a.Vehicle)
                    .Include(a => a.Data)
                    .Include(a => a.Anomalies)
                    .Include(a => a.Predictions)
                    .Include(a => a.Patterns)
                    .FirstOrDefaultAsync(a => a.Id == analysisId) ?? throw new InvalidOperationException("Analysis not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting analysis by id {AnalysisId}", analysisId);
                throw;
            }
        }

        public async Task<TrendAnalysis> GetTrendAnalysis(int analysisId)
        {
            var analysis = await _context.TrendAnalyses
                .FirstOrDefaultAsync(t => t.DobackAnalysisId == analysisId);

            if (analysis == null)
                throw new KeyNotFoundException($"Trend analysis not found for analysis {analysisId}");

            return analysis;
        }

        public async Task<List<DobackAnalysis>> GetAnalyses(int vehicleId)
        {
            return await _context.DobackAnalyses
                .Where(a => a.VehicleId == vehicleId)
                .OrderByDescending(a => a.Timestamp)
                .ToListAsync();
        }
    }
} 