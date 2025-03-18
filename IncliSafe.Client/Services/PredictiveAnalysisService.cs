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
using Microsoft.Extensions.Logging;

namespace IncliSafe.Client.Services
{
    public class PredictiveAnalysisService : IPredictiveAnalysisService
    {
        private readonly IDobackAnalysisService _dobackService;
        private readonly HttpClient _httpClient;
        private readonly ILogger<PredictiveAnalysisService> _logger;
        private const string BaseUrl = "api/analysis";

        public PredictiveAnalysisService(IDobackAnalysisService dobackService, HttpClient httpClient, ILogger<PredictiveAnalysisService> logger)
        {
            _dobackService = dobackService;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<PredictionResult> PredictStability(int vehicleId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<PredictionResult>($"api/predictive/stability/{vehicleId}?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
                return response ?? new PredictionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error predicting stability for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<List<Anomaly>> DetectAnomalies(int vehicleId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<Anomaly>>($"api/predictive/anomalies/{vehicleId}?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
                return response ?? new List<Anomaly>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error detecting anomalies for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<TrendAnalysis> AnalyzeTrends(int vehicleId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<TrendAnalysis>($"api/predictive/trends/{vehicleId}?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
                return response ?? new TrendAnalysis();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error analyzing trends for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<List<Pattern>> DetectPatterns(int vehicleId, DateTime startDate, DateTime endDate)
        {
            return new List<Pattern>();
        }

        public async Task<List<AnalysisPrediction>> GetPredictions(int vehicleId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<AnalysisPrediction>>($"api/predictive/predictions/{vehicleId}");
                return response ?? new List<AnalysisPrediction>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting predictions for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<AnalysisPrediction> GetPredictionAsync(int vehicleId)
        {
            return await _httpClient.GetFromJsonAsync<AnalysisPrediction>($"{BaseUrl}/predict/{vehicleId}")
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