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
using IncliSafe.Shared.Models.Notifications;
using IncliSafe.Shared.Models.DTOs;
using IncliSafe.Shared.DTOs;

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

        // Implementación de los métodos de la interfaz
        public async Task<DashboardMetrics> GetDashboardMetrics(int vehicleId)
        {
            try
            {
                var metrics = await _analysisService.GetDashboardMetricsAsync(vehicleId);
                return metrics;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dashboard metrics for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<List<DobackAnalysis>> GetAnalysisHistoryAsync(int vehicleId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var analyses = await _context.DobackAnalyses
                    .Where(a => a.VehicleId == vehicleId && a.AnalysisDate >= startDate && a.AnalysisDate <= endDate)
                    .OrderByDescending(a => a.AnalysisDate)
                    .AsNoTracking()
                    .ToListAsync();

                return analyses;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting analysis history for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<DobackAnalysis?> GetAnalysis(int id)
        {
            return await _context.DobackAnalyses
                .Include(a => a.Vehicle)
                .Include(a => a.Data)
                .Include(a => a.Anomalies)
                .Include(a => a.Predictions)
                .Include(a => a.Patterns)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<DobackAnalysis> CreateAnalysisAsync(int vehicleId, List<DobackData> data)
        {
            try
            {
                var analysis = await _analysisService.CreateAnalysisAsync(vehicleId, data);
                return analysis;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating analysis for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<bool> UpdateAnalysisAsync(DobackAnalysis analysis)
        {
            _context.Entry(analysis).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAnalysisAsync(int id)
        {
            var analysis = await _context.DobackAnalyses.FindAsync(id);
            if (analysis == null) return false;
            
            _context.DobackAnalyses.Remove(analysis);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<PatternDetails>> GetDetectedPatternsAsync(int vehicleId, DateTime startDate, DateTime endDate)
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
                _logger.LogError(ex, "Error getting detected patterns for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<TrendAnalysis> GetTrendAnalysisAsync(int vehicleId)
        {
            // Implementación del análisis de tendencias
            return new TrendAnalysis();
        }

        public async Task<List<DobackData>> GetDobackDataAsync(int analysisId)
        {
            var analysis = await _context.DobackAnalyses
                .Include(a => a.Data)
                .FirstOrDefaultAsync(a => a.Id == analysisId);

            return analysis?.Data.OrderBy(d => d.Timestamp).ToList() ?? new List<DobackData>();
        }

        public async Task<List<DobackData>> GetDobackData(int vehicleId, DateTime start, DateTime end)
        {
            return await _context.DobackData
                .Where(d => d.Analysis != null && 
                       d.Analysis.VehicleId == vehicleId &&
                       d.Timestamp >= start &&
                       d.Timestamp <= end)
                .OrderBy(d => d.Timestamp)
                .ToListAsync();
        }

        public async Task<List<Alert>> GetRecentAlertsAsync(int vehicleId, int count = 10)
        {
            try
            {
                var alerts = await _context.Alerts
                    .Where(a => a.VehicleId == vehicleId)
                    .OrderByDescending(a => a.CreatedAt)
                    .Take(count)
                    .AsNoTracking()
                    .ToListAsync();

                return alerts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting recent alerts for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<List<Anomaly>> GetRecentAnomalies(int vehicleId)
        {
            return await _context.Anomalies
                .Where(a => a.VehicleId == vehicleId)
                .OrderByDescending(a => a.DetectedAt)
                .Take(10)
                .ToListAsync();
        }

        public async Task<TrendAnalysis> GetTrendAnalysis(int vehicleId)
        {
            // Implementación del análisis de tendencias
            return new TrendAnalysis();
        }

        public async Task<AnalysisResult> GetAnalysisResult(int analysisId)
        {
            // Implementación del resultado del análisis
            return new AnalysisResult();
        }

        public async Task<List<AnalysisPrediction>> GetPredictionsAsync(int vehicleId, PredictionType type)
        {
            try
            {
                var predictions = await _predictiveService.GetPredictionsAsync(vehicleId, type);
                return predictions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting predictions for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<DobackAnalysis> ProcessDobackDataAsync(List<DobackData> data)
        {
            var analysis = new DobackAnalysis
            {
                Data = data,
                Timestamp = DateTime.UtcNow,
                VehicleId = data.First().CycleId,
                StabilityIndex = Convert.ToDecimal(data.Average(d => d.StabilityIndex)),
                DetectedPatterns = new List<IncliSafe.Shared.Models.Patterns.DetectedPattern>()
            };

            // Detectar patrones
            var patterns = await DetectPatternsAsync(data);
            analysis.DetectedPatterns = patterns;

            return analysis;
        }

        private AnomalyType DetermineAnomalyType(Anomaly anomaly)
        {
            if (anomaly.Deviation > 0.8)
                return AnomalyType.Performance;
            if (anomaly.Deviation > 0.5)
                return AnomalyType.Safety;
            return AnomalyType.Stability;
        }

        private NotificationSeverity CalculateAnomalySeverity(Anomaly anomaly)
        {
            return anomaly.Deviation switch
            {
                > 0.8 => NotificationSeverity.Critical,
                > 0.5 => NotificationSeverity.Warning,
                _ => NotificationSeverity.Info
            };
        }

        private async Task<List<IncliSafe.Shared.Models.Patterns.DetectedPattern>> DetectPatternsAsync(List<DobackData> data)
        {
            var patterns = new List<IncliSafe.Shared.Models.Patterns.DetectedPattern>();
            
            // Detectar patrones de estabilidad
            if (data.Any(d => d.StabilityIndex < 0.5))
            {
                patterns.Add(new IncliSafe.Shared.Models.Patterns.DetectedPattern
                {
                    PatternName = "Baja Estabilidad",
                    Description = "Se detectó un patrón de baja estabilidad",
                    Type = "Stability",
                    DetectionTime = DateTime.UtcNow,
                    ConfidenceScore = 0.85,
                    DetectedValues = data.Select(d => d.StabilityIndex).ToList()
                });
            }

            return patterns;
        }

        private async Task<List<Anomaly>> DetectAnomaliesAsync(List<DobackData> data)
        {
            var anomalies = new List<Anomaly>();
            
            // Detectar anomalías en aceleración
            var avgAccX = data.Average(d => d.AccelerationX);
            if (Math.Abs(avgAccX) > 2.0)
            {
                anomalies.Add(new Anomaly
                {
                    Description = "Aceleración lateral anormal",
                    ExpectedValue = 0,
                    ActualValue = avgAccX,
                    Deviation = Math.Abs(avgAccX),
                    Type = AnomalyType.Acceleration
                });
            }

            return anomalies;
        }

        public async Task<decimal> CalculateStabilityIndex(List<DobackData> data)
        {
            if (!data.Any()) return 0;

            var avgAccX = Convert.ToDecimal(data.Average(d => d.AccelerationX));
            var avgAccY = Convert.ToDecimal(data.Average(d => d.AccelerationY));
            var avgAccZ = Convert.ToDecimal(data.Average(d => d.AccelerationZ));

            return Math.Round(
                (1m - (Math.Abs(avgAccX) + Math.Abs(avgAccY) + Math.Abs(avgAccZ)) / 3m) * 100m, 
                2);
        }

        public List<decimal> GetDataSeries(ICollection<DobackData> data, string property)
        {
            if (data == null || !data.Any())
                return new List<decimal>();

            return data.Select(d => property switch
            {
                "AccelerationX" => d.AccelerationX,
                "AccelerationY" => d.AccelerationY,
                "AccelerationZ" => d.AccelerationZ,
                "Roll" => d.Roll,
                "Pitch" => d.Pitch,
                "Yaw" => d.Yaw,
                "Speed" => d.Speed,
                "StabilityIndex" => d.StabilityIndex,
                _ => 0M
            }).ToList();
        }

        public decimal GetAverageValue(ICollection<DobackData> data, string property)
        {
            var series = GetDataSeries(data, property);
            return series.Any() ? series.Average() : 0M;
        }

        private bool IsStabilityAnomaly(decimal value)
        {
            return value < 0.5M;
        }

        private bool IsSpeedAnomaly(decimal value, decimal threshold)
        {
            if (value > 1.5M || value < -0.5M)
            {
                return true;
            }
            return false;
        }

        private async Task<List<decimal>> GetHistoricalValues(int vehicleId, string metric)
        {
            var data = await _context.DobackData
                .Where(d => d.CycleId == vehicleId)
                .OrderBy(d => d.Timestamp)
                .ToListAsync();

            return metric switch
            {
                "stability" => data.Select(d => d.StabilityIndex).ToList(),
                "speed" => data.Select(d => d.Speed).ToList(),
                _ => new List<decimal>()
            };
        }

        private List<double> ConvertToDoubleList(List<decimal> values)
        {
            return values.Select(v => Convert.ToDouble(v)).ToList();
        }

        public async Task<AnalysisPrediction> GeneratePredictionAsync(int vehicleId)
        {
            var prediction = new AnalysisPrediction
            {
                VehicleId = vehicleId,
                PredictedAt = DateTime.UtcNow,
                Type = PredictionType.Stability
            };
            
            _context.AnalysisPredictions.Add(prediction);
            await _context.SaveChangesAsync();
            return prediction;
        }

        public async Task<CoreMetrics> GetMetricsAsync()
        {
            var metrics = new CoreMetrics
            {
                TotalAnalyses = await _context.DobackAnalyses.CountAsync(),
                TotalAnomalies = await _context.Anomalies.CountAsync(),
                TotalPredictions = await _context.AnalysisPredictions.CountAsync(),
                TotalPatterns = await _context.PatternDetails.CountAsync(),
                AverageStabilityScore = await _context.DobackAnalyses.AverageAsync(a => a.StabilityScore),
                AverageSafetyScore = await _context.DobackAnalyses.AverageAsync(a => a.SafetyScore),
                AverageMaintenanceScore = await _context.DobackAnalyses.AverageAsync(a => a.MaintenanceScore)
            };

            // Get trend data for the last 30 days
            var startDate = DateTime.UtcNow.AddDays(-30);
            var analyses = await _context.DobackAnalyses
                .Where(a => a.AnalysisDate >= startDate)
                .OrderBy(a => a.AnalysisDate)
                .ToListAsync();

            metrics.StabilityTrend = analyses.Select(a => new TrendData
            {
                Timestamp = a.AnalysisDate,
                Value = a.StabilityScore
            }).ToList();

            metrics.SafetyTrend = analyses.Select(a => new TrendData
            {
                Timestamp = a.AnalysisDate,
                Value = a.SafetyScore
            }).ToList();

            metrics.MaintenanceTrend = analyses.Select(a => new TrendData
            {
                Timestamp = a.AnalysisDate,
                Value = a.MaintenanceScore
            }).ToList();

            return metrics;
        }

        public async Task<DobackAnalysis> GetLatestAnalysisAsync(int vehicleId)
        {
            return await _context.DobackAnalyses
                .Where(a => a.VehicleId == vehicleId)
                .OrderByDescending(a => a.Timestamp)
                .FirstOrDefaultAsync();
        }

        public async Task<List<DobackAnalysis>> GetAnalyses(int vehicleId)
        {
            return await _context.DobackAnalyses
                .Where(a => a.VehicleId == vehicleId)
                .OrderByDescending(a => a.Timestamp)
                .ToListAsync();
        }

        public async Task<TrendAnalysis> GetTrendAnalysis(int analysisId)
        {
            var analysis = await _context.TrendAnalyses
                .FirstOrDefaultAsync(t => t.DobackAnalysisId == analysisId);

            if (analysis == null)
                throw new KeyNotFoundException($"Trend analysis not found for analysis {analysisId}");

            return analysis;
        }

        public async Task<List<Anomaly>> GetAnomalies(int vehicleId)
        {
            return await _context.Anomalies
                .Where(a => a.VehicleId == vehicleId)
                .OrderByDescending(a => a.DetectedAt)
                .ToListAsync();
        }

        public async Task<List<Pattern>> GetPatterns(int vehicleId)
        {
            return await _context.Patterns
                .Where(p => p.VehicleId == vehicleId)
                .OrderByDescending(p => p.DetectedAt)
                .ToListAsync();
        }

        public async Task<List<AnalysisPrediction>> GetAnalysisPredictions(int vehicleId)
        {
            return await _context.Predictions
                .Where(p => p.VehicleId == vehicleId)
                .OrderByDescending(p => p.Timestamp)
                .ToListAsync();
        }

        public async Task<DobackAnalysis?> GetLatestAnalysis(int vehicleId)
        {
            return await _context.DobackAnalyses
                .Include(a => a.Vehicle)
                .Include(a => a.Data)
                .Include(a => a.Anomalies)
                .Include(a => a.Predictions)
                .Include(a => a.Patterns)
                .Where(a => a.VehicleId == vehicleId)
                .OrderByDescending(a => a.AnalysisDate)
                .FirstOrDefaultAsync();
        }

        public async Task<DobackAnalysis> AnalyzeDobackAsync(int vehicleId, DobackAnalysisDTO analysisDto)
        {
            try
            {
                var analysis = await _analysisService.AnalyzeDobackAsync(vehicleId, analysisDto);
                return analysis;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error analyzing doback for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<TrendAnalysis> AnalyzeTrendsAsync(int vehicleId, DateTime startDate, DateTime endDate)
        {
            var vehicle = await _context.Vehiculos.FindAsync(vehicleId)
                ?? throw new InvalidOperationException($"Vehicle with ID {vehicleId} not found.");

            var analyses = await _context.DobackAnalyses
                .Where(a => a.VehicleId == vehicleId && a.AnalysisDate >= startDate && a.AnalysisDate <= endDate)
                .OrderBy(a => a.AnalysisDate)
                .ToListAsync();

            if (!analyses.Any())
                throw new InvalidOperationException("No analyses found for the specified period.");

            var trendAnalysis = new TrendAnalysis
            {
                VehicleId = vehicleId,
                Vehicle = vehicle,
                AnalysisDate = DateTime.UtcNow,
                Type = AnalysisType.Trend,
                StartDate = startDate,
                EndDate = endDate,
                StabilityScore = analyses.Average(a => a.StabilityScore),
                SafetyScore = analyses.Average(a => a.SafetyScore),
                MaintenanceScore = analyses.Average(a => a.MaintenanceScore),
                TrendValue = CalculateTrendValue(analyses),
                Seasonality = CalculateSeasonality(analyses),
                Correlation = CalculateCorrelation(analyses),
                Data = analyses.Select(a => new TrendData
                {
                    Timestamp = a.AnalysisDate,
                    Value = (a.StabilityScore + a.SafetyScore + a.MaintenanceScore) / 3
                }).ToList()
            };

            _context.TrendAnalyses.Add(trendAnalysis);
            await _context.SaveChangesAsync();

            return trendAnalysis;
        }

        private decimal CalculateTrendValue(List<DobackAnalysis> analyses)
        {
            // Simple linear regression for trend
            var n = analyses.Count;
            var sumX = 0.0m;
            var sumY = 0.0m;
            var sumXY = 0.0m;
            var sumX2 = 0.0m;

            for (var i = 0; i < n; i++)
            {
                var x = i;
                var y = (analyses[i].StabilityScore + analyses[i].SafetyScore + analyses[i].MaintenanceScore) / 3;
                sumX += x;
                sumY += y;
                sumXY += x * y;
                sumX2 += x * x;
            }

            var slope = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX);
            return slope;
        }

        private decimal CalculateSeasonality(List<DobackAnalysis> analyses)
        {
            // Simple seasonality calculation based on weekly patterns
            var weeklyAverages = analyses
                .GroupBy(a => a.AnalysisDate.DayOfWeek)
                .Select(g => g.Average(a => (a.StabilityScore + a.SafetyScore + a.MaintenanceScore) / 3))
                .ToList();

            var overallAverage = weeklyAverages.Average();
            var seasonalityIndex = weeklyAverages.Max(w => Math.Abs((decimal)(w - overallAverage)));

            return seasonalityIndex;
        }

        private decimal CalculateCorrelation(List<DobackAnalysis> analyses)
        {
            // Calculate correlation between stability and safety scores
            var n = analyses.Count;
            var sumX = analyses.Sum(a => a.StabilityScore);
            var sumY = analyses.Sum(a => a.SafetyScore);
            var sumXY = analyses.Sum(a => a.StabilityScore * a.SafetyScore);
            var sumX2 = analyses.Sum(a => a.StabilityScore * a.StabilityScore);
            var sumY2 = analyses.Sum(a => a.SafetyScore * a.SafetyScore);

            var correlation = (n * sumXY - sumX * sumY) /
                (decimal)Math.Sqrt((double)((n * sumX2 - sumX * sumX) * (n * sumY2 - sumY * sumY)));

            return correlation;
        }

        public async Task<DobackAnalysis> GetAnalysisByIdAsync(int analysisId)
        {
            try
            {
                var analysis = await _context.DobackAnalyses
                    .Include(a => a.Data)
                    .FirstOrDefaultAsync(a => a.Id == analysisId);

                return analysis;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting analysis by id {AnalysisId}", analysisId);
                throw;
            }
        }

        public async Task<TrendAnalysis> GetTrendAnalysisAsync(int vehicleId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var analysis = await _analysisService.GetTrendAnalysisAsync(vehicleId, startDate, endDate);
                return analysis;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting trend analysis for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }
    }
} 