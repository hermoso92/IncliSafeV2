using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using IncliSafe.Client.Services.Interfaces;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Shared.DTOs;
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
        private readonly ILogger<DobackService> _logger;
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/doback";

        public DobackService(ILogger<DobackService> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<DashboardMetrics> GetDashboardMetricsAsync(int vehicleId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<DashboardMetrics>($"api/doback/metrics/{vehicleId}");
                return response ?? new DashboardMetrics();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener métricas del dashboard para el vehículo {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<List<DobackAnalysis>> GetAnalysisHistoryAsync(int vehicleId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<DobackAnalysis>>($"api/doback/history/{vehicleId}?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
                return response ?? new List<DobackAnalysis>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener historial de análisis para el vehículo {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<DobackAnalysis> CreateAnalysisAsync(int vehicleId, List<DobackData> data)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"api/doback/create/{vehicleId}", data);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<DobackAnalysis>() ?? throw new InvalidOperationException("No se pudo crear el análisis");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear análisis para el vehículo {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<List<AnalysisPattern>> GetDetectedPatternsAsync(int vehicleId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<AnalysisPattern>>($"api/doback/patterns/{vehicleId}?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
                return response ?? new List<AnalysisPattern>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener patrones detectados para el vehículo {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<List<AnalysisAlert>> GetRecentAlertsAsync(int vehicleId, int count)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<AnalysisAlert>>($"api/doback/alerts/{vehicleId}?count={count}");
                return response ?? new List<AnalysisAlert>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener alertas recientes para el vehículo {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<List<AnalysisPrediction>> GetPredictionsAsync(int vehicleId, PredictionType type)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<AnalysisPrediction>>($"api/doback/predictions/{vehicleId}?type={type}");
                return response ?? new List<AnalysisPrediction>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener predicciones para el vehículo {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<DobackAnalysis> GetAnalysisByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<DobackAnalysis>($"api/doback/analysis/{id}");
                return response ?? throw new InvalidOperationException($"No se encontró el análisis con ID {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener análisis con ID {AnalysisId}", id);
                throw;
            }
        }

        public async Task<TrendAnalysis> GetTrendAnalysisAsync(int vehicleId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<TrendAnalysis>($"api/doback/trends/{vehicleId}?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
                return response ?? throw new InvalidOperationException($"No se encontró el análisis de tendencias para el vehículo {vehicleId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener análisis de tendencias para el vehículo {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<DobackAnalysis> AnalyzeDobackAsync(int vehicleId, DobackAnalysisDTO analysis)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"api/doback/analyze/{vehicleId}", analysis);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<DobackAnalysis>() ?? throw new InvalidOperationException("No se pudo realizar el análisis");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al analizar datos para el vehículo {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<DobackAnalysis> GetAnalysisAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<DobackAnalysis>($"api/doback/{id}");
                return response ?? throw new InvalidOperationException($"No se encontró el análisis con ID {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener análisis con ID {AnalysisId}", id);
                throw;
            }
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

        public async Task<TrendAnalysis> AnalyzeTrendsAsync(int vehicleId, DateTime startDate, DateTime endDate)
        {
            var trendDto = new TrendAnalysisDTO { StartDate = startDate, EndDate = endDate };
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/trends/{vehicleId}", trendDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TrendAnalysis>() ?? new();
        }
    }
} 