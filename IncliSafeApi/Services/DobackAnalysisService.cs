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
    }
} 