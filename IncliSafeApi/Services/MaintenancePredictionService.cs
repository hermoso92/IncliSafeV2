using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using IncliSafe.Shared.Models.DTOs;
using IncliSafe.Shared.Models.Analysis;
using IncliSafeApi.Data;
using IncliSafeApi.Services.Interfaces;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Analysis.Core;

namespace IncliSafeApi.Services
{
    public class MaintenancePredictionService : IMaintenancePredictionService
    {
        private readonly ApplicationDbContext _context;
        private readonly ITrendAnalysisService _trendService;
        private readonly IAlertGenerationService _alertService;
        private readonly ILogger<MaintenancePredictionService> _logger;

        public MaintenancePredictionService(
            ApplicationDbContext context,
            ITrendAnalysisService trendService,
            IAlertGenerationService alertService,
            ILogger<MaintenancePredictionService> logger)
        {
            _context = context;
            _trendService = trendService;
            _alertService = alertService;
            _logger = logger;
        }

        public async Task<MaintenancePredictionDTO> PredictMaintenanceAsync(int vehicleId)
        {
            try
            {
                var trends = await _trendService.AnalyzeVehicleTrendsAsync(vehicleId);
                var recentAnalyses = await GetRecentAnalyses(vehicleId);
                var lastInspection = await GetLastInspection(vehicleId);

                var prediction = new MaintenancePredictionDTO
                {
                    VehicleId = vehicleId,
                    LastInspectionDate = lastInspection?.Fecha,
                    PredictedMaintenanceDate = PredictNextMaintenanceDate(recentAnalyses, lastInspection),
                    MaintenanceProbability = CalculateMaintenanceProbability(trends, recentAnalyses),
                    RiskLevel = CalculateRiskLevel(trends, recentAnalyses),
                    Recommendations = GenerateRecommendations(trends, recentAnalyses, lastInspection)
                };

                await SavePrediction(prediction);
                await GenerateAlertIfNeeded(prediction);

                return prediction;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error predicting maintenance for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }

        private async Task<List<DobackAnalysis>> GetRecentAnalyses(int vehicleId)
        {
            return await _context.DobackAnalyses
                .Where(a => a.VehicleId == vehicleId)
                .OrderByDescending(a => a.Timestamp)
                .Take(30)
                .ToListAsync();
        }

        private async Task<Inspeccion?> GetLastInspection(int vehicleId)
        {
            return await _context.Inspecciones
                .Where(i => i.VehiculoId == vehicleId)
                .OrderByDescending(i => i.Fecha)
                .FirstOrDefaultAsync();
        }

        private DateTime PredictNextMaintenanceDate(List<DobackAnalysis> analyses, Inspeccion? lastInspection)
        {
            if (!analyses.Any())
                return DateTime.UtcNow.AddMonths(3);

            var degradationRate = CalculateDegradationRate(analyses);
            var baseInterval = TimeSpan.FromDays(90); // 3 meses por defecto

            if (degradationRate > 0.1M)
                baseInterval = TimeSpan.FromDays(60);
            else if (degradationRate > 0.05M)
                baseInterval = TimeSpan.FromDays(75);

            var lastDate = lastInspection?.Fecha ?? DateTime.UtcNow;
            return lastDate.Add(baseInterval);
        }

        private decimal CalculateDegradationRate(List<DobackAnalysis> analyses)
        {
            if (analyses.Count < 2)
                return 0;

            var recentScores = analyses.Take(5).Select(a => a.StabilityScore).ToList();
            var oldScores = analyses.Skip(5).Take(5).Select(a => a.StabilityScore).ToList();

            if (!recentScores.Any() || !oldScores.Any())
                return 0;

            return (oldScores.Average() - recentScores.Average()) / oldScores.Average();
        }

        private decimal CalculateMaintenanceProbability(TrendAnalysisDTO trends, List<DobackAnalysis> analyses)
        {
            if (!analyses.Any())
                return 0.5M;

            var factors = new List<decimal>();

            // Factor de tendencia
            if (trends.StabilityTrend == TrendDirection.Declining)
                factors.Add(0.7M);
            else if (trends.StabilityTrend == TrendDirection.Improving)
                factors.Add(0.3M);
            else
                factors.Add(0.5M);

            // Factor de puntuación reciente
            var recentScore = analyses.First().StabilityScore;
            factors.Add(1 - recentScore);

            // Factor de tiempo desde última inspección
            if (trends.LastAnalysisDate.HasValue)
            {
                var daysSinceAnalysis = (DateTime.UtcNow - trends.LastAnalysisDate.Value).Days;
                factors.Add(Math.Min(1, daysSinceAnalysis / 90M));
            }

            return factors.Average();
        }

        private RiskLevel CalculateRiskLevel(TrendAnalysisDTO trends, List<DobackAnalysis> analyses)
        {
            var probability = CalculateMaintenanceProbability(trends, analyses);
            return probability switch
            {
                > 0.7M => RiskLevel.High,
                > 0.4M => RiskLevel.Medium,
                _ => RiskLevel.Low
            };
        }

        private List<string> GenerateRecommendations(
            TrendAnalysisDTO trends,
            List<DobackAnalysis> analyses,
            Inspeccion? lastInspection)
        {
            var recommendations = new List<string>();

            if (lastInspection == null || (DateTime.UtcNow - lastInspection.Fecha).Days > 90)
            {
                recommendations.Add("Se recomienda programar una inspección general.");
            }

            if (trends.StabilityTrend == TrendDirection.Declining)
            {
                recommendations.Add("Los indicadores de estabilidad muestran una tendencia negativa. Se sugiere revisión preventiva.");
            }

            if (analyses.Any() && analyses.First().StabilityScore < 0.7M)
            {
                recommendations.Add("El último análisis muestra valores subóptimos. Considerar mantenimiento próximo.");
            }

            return recommendations;
        }

        private async Task SavePrediction(MaintenancePredictionDTO prediction)
        {
            var entity = new Prediction
            {
                VehicleId = prediction.VehicleId,
                PredictionType = PredictionType.Maintenance,
                PredictedDate = prediction.PredictedMaintenanceDate,
                Probability = prediction.MaintenanceProbability,
                RiskLevel = prediction.RiskLevel,
                CreatedAt = DateTime.UtcNow,
                Recommendations = string.Join("|", prediction.Recommendations)
            };

            _context.Predictions.Add(entity);
            await _context.SaveChangesAsync();
        }

        private async Task GenerateAlertIfNeeded(MaintenancePredictionDTO prediction)
        {
            if (prediction.RiskLevel == RiskLevel.High || prediction.MaintenanceProbability > 0.8M)
            {
                var alert = new VehicleAlertDTO
                {
                    VehicleId = prediction.VehicleId,
                    Title = "Mantenimiento Preventivo Recomendado",
                    Message = $"Alta probabilidad de requerir mantenimiento. Fecha sugerida: {prediction.PredictedMaintenanceDate:d}",
                    Type = AlertType.Maintenance,
                    Severity = AlertSeverity.Warning
                };

                await _alertService.CreateAlertAsync(prediction.VehicleId, alert);
            }
        }
    }
} 