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
using IncliSafe.Shared.Models.Analysis.Core;
using IncliSafe.Shared.Models;
using Anomaly = IncliSafe.Shared.Models.Analysis.Core.Anomaly;
using IncliSafe.Shared.Models.DTOs;

namespace IncliSafe.Client.Services
{
    public class DobackService : ServiceBase, IDobackService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/doback";
        private new readonly ILogger<DobackService> _logger;

        public DobackService(
            HttpClient httpClient,
            ISnackbar snackbar,
            ILogger<DobackService> logger,
            CacheService cache) : base(httpClient, snackbar, logger, cache)
        {
            _httpClient = httpClient;
            _logger = logger;
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

        public async Task<List<double>> GetDataSeries(ICollection<DobackData> data, string property)
        {
            return data.Select(d => property switch
            {
                "AccelerationX" => Convert.ToDouble(d.AccelerationX),
                "AccelerationY" => Convert.ToDouble(d.AccelerationY),
                "AccelerationZ" => Convert.ToDouble(d.AccelerationZ),
                "Roll" => Convert.ToDouble(d.Roll),
                "Pitch" => Convert.ToDouble(d.Pitch),
                "Yaw" => Convert.ToDouble(d.Yaw),
                "Speed" => Convert.ToDouble(d.Speed),
                "StabilityIndex" => Convert.ToDouble(d.StabilityIndex),
                _ => 0.0
            }).ToList();
        }

        public async Task<double> GetAverageValue(ICollection<DobackData> data, string property)
        {
            var series = await GetDataSeries(data, property);
            return series.Any() ? series.Average() : 0;
        }

        public async Task<DobackAnalysis?> GetAnalysis(int id)
        {
            return await _httpClient.GetFromJsonAsync<DobackAnalysis>($"{BaseUrl}/{id}");
        }

        public async Task<DobackAnalysis?> GetLatestAnalysis(int vehicleId)
        {
            return await _httpClient.GetFromJsonAsync<DobackAnalysis>($"{BaseUrl}/vehicle/{vehicleId}/latest");
        }

        public async Task<List<AnalysisPrediction>> GetPredictions(int vehicleId)
        {
            return await _httpClient.GetFromJsonAsync<List<AnalysisPrediction>>($"{BaseUrl}/vehicle/{vehicleId}/predictions") ?? new();
        }

        public async Task<CoreMetrics> GetMetricsAsync()
        {
            return await _httpClient.GetFromJsonAsync<CoreMetrics>($"{BaseUrl}/metrics") ?? new();
        }

        public async Task<DobackAnalysis> AnalyzeDobackAsync(int vehicleId, DobackAnalysisDTO analysisDto)
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/analyze/{vehicleId}", analysisDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<DobackAnalysis>() ?? new();
        }

        public async Task<TrendAnalysis> AnalyzeTrendsAsync(int vehicleId, DateTime startDate, DateTime endDate)
        {
            var trendDto = new TrendAnalysisDTO { StartDate = startDate, EndDate = endDate };
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/trends/{vehicleId}", trendDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TrendAnalysis>() ?? new();
        }
    }
} 