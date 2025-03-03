using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Client.Services.Interfaces;
using IncliSafe.Shared.Models.Analysis.Core;
using CorePredictionType = IncliSafe.Shared.Models.Analysis.Core.PredictionType;
using System.Net.Http;
using System.Net.Http.Json;
using IncliSafe.Shared.Models.DTOs;

namespace IncliSafe.Client.Services
{
    public class PredictiveAnalysisService : IPredictiveAnalysisService
    {
        private readonly IDobackAnalysisService _dobackService;
        private readonly HttpClient _http;
        private const string BaseUrl = "api/analysis";

        public PredictiveAnalysisService(IDobackAnalysisService dobackService, HttpClient http)
        {
            _dobackService = dobackService;
            _http = http;
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

        public async Task<TrendAnalysis> AnalyzeTrends(int vehicleId, DateTime startDate, DateTime endDate)
        {
            var trendDto = new TrendAnalysisDTO { StartDate = startDate, EndDate = endDate };
            var response = await _http.PostAsJsonAsync($"{BaseUrl}/trends/{vehicleId}", trendDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TrendAnalysis>() ?? new();
        }

        public async Task<List<Pattern>> DetectPatterns(int vehicleId, DateTime startDate, DateTime endDate)
        {
            return new List<Pattern>();
        }

        public async Task<List<AnalysisPrediction>> GetPredictions(int vehicleId)
        {
            return await _http.GetFromJsonAsync<List<AnalysisPrediction>>($"{BaseUrl}/predictions/{vehicleId}") ?? new();
        }

        public async Task<AnalysisPrediction> GetPredictionAsync(int vehicleId)
        {
            return await _http.GetFromJsonAsync<AnalysisPrediction>($"{BaseUrl}/predict/{vehicleId}")
                ?? new AnalysisPrediction();
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