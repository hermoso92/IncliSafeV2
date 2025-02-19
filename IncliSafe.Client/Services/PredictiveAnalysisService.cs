using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Client.Services.Interfaces;
using IncliSafe.Shared.Models.Analysis.Core;
using CorePredictionType = IncliSafe.Shared.Models.Analysis.Core.PredictionType;

namespace IncliSafe.Client.Services
{
    public class PredictiveAnalysisService : IPredictiveAnalysisService
    {
        private readonly IDobackAnalysisService _dobackService;

        public PredictiveAnalysisService(IDobackAnalysisService dobackService)
        {
            _dobackService = dobackService;
        }

        public async Task<PredictionResult> PredictStability(int vehicleId, DateTime startDate, DateTime endDate)
        {
            // Implementación del cliente
            return new PredictionResult();
        }

        public async Task<List<Anomaly>> DetectAnomalies(int vehicleId, DateTime startDate, DateTime endDate)
        {
            return new List<Anomaly>();  // Implementar lógica real
        }

        public async Task<IncliSafe.Shared.Models.Analysis.Core.TrendAnalysis> AnalyzeTrends(int vehicleId, DateTime startDate, DateTime endDate)
        {
            var data = await _dobackService.GetDobackDataAsync(vehicleId);
            return new IncliSafe.Shared.Models.Analysis.Core.TrendAnalysis();
        }

        public async Task<List<Pattern>> DetectPatterns(int vehicleId, DateTime startDate, DateTime endDate)
        {
            return new List<Pattern>();
        }

        public async Task<List<AnalysisPrediction>> GetPredictions(int analysisId)
        {
            return await _dobackService.GetPredictionsAsync(analysisId);
        }

        private CorePredictionType GetPredictionType(decimal value)
        {
            return value switch
            {
                > 0.8m => CorePredictionType.Stability,
                > 0.6m => CorePredictionType.Safety,
                > 0.4m => CorePredictionType.Maintenance,
                > 0.2m => CorePredictionType.Performance,
                _ => CorePredictionType.Anomaly
            };
        }
    }
} 