using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IncliSafeApi.Data;
using IncliSafeApi.Services.Interfaces;
using IncliSafe.Shared.Models.Analysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using IncliSafe.Shared.Models;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Patterns;
using IncliSafe.Shared.Models.Analysis.Core;
using CoreMetrics = IncliSafe.Shared.Models.Analysis.Core.DashboardMetrics;
using CoreAnalysisPrediction = IncliSafe.Shared.Models.Analysis.Core.AnalysisPrediction;
using CorePredictionType = IncliSafe.Shared.Models.Analysis.Core.PredictionType;

namespace IncliSafeApi.Services
{
    public class DobackService : IDobackService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DobackService> _logger;

        public DobackService(ApplicationDbContext context, ILogger<DobackService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Implementación de los métodos de la interfaz
        public async Task<DashboardMetrics> GetDashboardMetrics()
        {
            return new DashboardMetrics();
        }

        public async Task<List<DobackAnalysis>> GetAnalysisHistoryAsync(int vehicleId)
        {
            return await _context.DobackAnalyses
                .Where(a => a.VehicleId == vehicleId)
                .OrderByDescending(a => a.Timestamp)
                .ToListAsync();
        }

        public async Task<DobackAnalysis?> GetAnalysis(int id)
        {
            return await _context.DobackAnalyses
                .Include(a => a.Data)
                .Include(a => a.DetectedPatterns)
                .Include(a => a.Predictions)
                .Include(a => a.Result)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<DobackAnalysis> CreateAnalysisAsync(DobackAnalysis analysis)
        {
            _context.DobackAnalyses.Add(analysis);
            await _context.SaveChangesAsync();
            return analysis;
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

        public async Task<List<IncliSafe.Shared.Models.Patterns.DetectedPattern>> GetDetectedPatternsAsync(int analysisId)
        {
            var analysis = await _context.DobackAnalyses
                .Include(a => a.DetectedPatterns)
                .FirstOrDefaultAsync(a => a.Id == analysisId);
                
            return analysis?.DetectedPatterns?.ToList() ?? new List<IncliSafe.Shared.Models.Patterns.DetectedPattern>();
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

        public async Task<List<Alert>> GetRecentAlerts(int vehicleId)
        {
            return await _context.Alerts
                .Where(a => a.VehicleId == vehicleId)
                .OrderByDescending(a => a.Timestamp)
                .Take(10)
                .ToListAsync();
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

        public async Task<List<CoreAnalysisPrediction>> GetPredictions(int vehicleId)
        {
            return await _context.AnalysisPredictions
                .Where(p => p.VehicleId == vehicleId)
                .OrderByDescending(p => p.Timestamp)
                .ToListAsync();
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

        public async Task<CoreAnalysisPrediction> GeneratePredictionAsync(int vehicleId)
        {
            var prediction = new CoreAnalysisPrediction
            {
                VehicleId = vehicleId,
                Timestamp = DateTime.UtcNow,
                Type = CorePredictionType.Normal
            };
            
            _context.AnalysisPredictions.Add(prediction);
            await _context.SaveChangesAsync();
            return prediction;
        }

        public async Task<CoreMetrics> GetMetricsAsync()
        {
            return new CoreMetrics
            {
                StabilityScore = await CalculateAverageStabilityScore(),
                SafetyScore = await CalculateAverageSafetyScore(),
                Trends = await CalculateTrendMetrics()
            };
        }

        public async Task<DobackAnalysis> GetLatestAnalysisAsync(int vehicleId)
        {
            return await _context.DobackAnalyses
                .Where(a => a.VehicleId == vehicleId)
                .OrderByDescending(a => a.Timestamp)
                .FirstOrDefaultAsync();
        }
    }
} 