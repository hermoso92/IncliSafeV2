using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using IncliSafe.Client.Services.Interfaces;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Shared.Models.Notifications;
using Microsoft.Extensions.Logging;
using MudBlazor;

namespace IncliSafe.Client.Services
{
    public class DobackService : ServiceBase, IDobackService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/doback";

        public DobackService(
            HttpClient httpClient,
            ISnackbar snackbar,
            ILogger<DobackService> logger,
            CacheService cache) : base(httpClient, snackbar, logger, cache)
        {
            _httpClient = httpClient;
        }

        public async Task<DashboardMetrics> GetDashboardMetrics()
        {
            var result = await HandleRequestAsync(async () =>
            {
                return await _httpClient.GetFromJsonAsync<DashboardMetrics>($"{BaseUrl}/metrics");
            }, "Error al obtener métricas del dashboard");

            return result.Data ?? new DashboardMetrics();
        }

        public async Task<List<Alert>> GetRecentAlerts()
        {
            var result = await HandleRequestAsync(async () =>
            {
                return await _httpClient.GetFromJsonAsync<List<Alert>>($"{BaseUrl}/alerts/recent");
            }, "Error al obtener alertas recientes");

            return result.Data ?? new List<Alert>();
        }

        public async Task<List<Anomaly>> GetRecentAnomalies()
        {
            var result = await HandleRequestAsync(async () =>
            {
                return await _httpClient.GetFromJsonAsync<List<Anomaly>>($"{BaseUrl}/anomalies/recent");
            }, "Error al obtener anomalías recientes");

            return result.Data ?? new List<Anomaly>();
        }

        public async Task<AnalysisResult> GetAnalysisResult(int vehicleId)
        {
            var result = await HandleRequestAsync(async () =>
            {
                return await _httpClient.GetFromJsonAsync<AnalysisResult>($"{BaseUrl}/analysis/{vehicleId}");
            }, "Error al obtener resultado del análisis");

            return result.Data ?? new AnalysisResult();
        }

        public async Task<List<DobackData>> GetDobackData(int analysisId)
        {
            var result = await HandleRequestAsync(async () =>
            {
                return await _httpClient.GetFromJsonAsync<List<DobackData>>($"{BaseUrl}/data/{analysisId}");
            }, "Error al obtener datos Doback");

            return result.Data ?? new List<DobackData>();
        }

        public async Task<TrendAnalysis> GetTrendAnalysis(int analysisId)
        {
            var result = await HandleRequestAsync(async () =>
            {
                return await _httpClient.GetFromJsonAsync<TrendAnalysis>($"{BaseUrl}/trends/{analysisId}");
            }, "Error al obtener análisis de tendencias");

            return result.Data ?? new TrendAnalysis();
        }

        public async Task<PredictionResult> GetPredictions(int analysisId)
        {
            var result = await HandleRequestAsync(async () =>
            {
                return await _httpClient.GetFromJsonAsync<PredictionResult>($"{BaseUrl}/predictions/{analysisId}");
            }, "Error al obtener predicciones");

            return result.Data ?? new PredictionResult();
        }

        // ... resto de la implementación de los métodos
    }
} 