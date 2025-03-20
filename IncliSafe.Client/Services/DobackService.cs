using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using IncliSafe.Client.Services.Interfaces;
using IncliSafe.Shared.Models.Analysis.Core;
using IncliSafe.Shared.Models.Analysis.Extensions;
using IncliSafe.Shared.Models.Enums;
using Microsoft.Extensions.Logging;
using MudBlazor;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Shared.DTOs;
using IncliSafe.Shared.Models;
using Anomaly = IncliSafe.Shared.Models.Analysis.Core.Anomaly;
using IncliSafe.Shared.Models.DTOs;

namespace IncliSafe.Client.Services
{
    public class DobackService : ServiceBase, IDobackService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<DobackService> _logger;
        private const string BaseUrl = "api/doback";

        public DobackService(HttpClient httpClient, ILogger<DobackService> logger) : base(logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<DashboardMetrics> GetDashboardMetricsAsync(int vehicleId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<DashboardMetrics>($"api/doback/dashboard/{vehicleId}");
                return response ?? throw new Exception("No se pudieron obtener las métricas del dashboard");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las métricas del dashboard para el vehículo {VehicleId}", vehicleId);
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
                _logger.LogError(ex, "Error al obtener las predicciones para el vehículo {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<DobackAnalysis> GetAnalysisAsync(int vehicleId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<DobackAnalysis>($"api/doback/analysis/{vehicleId}");
                return response ?? throw new Exception("No se pudo obtener el análisis");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el análisis para el vehículo {VehicleId}", vehicleId);
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

        public async Task<DobackAnalysis> GetLatestAnalysisAsync(int vehicleId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<DobackAnalysis>($"api/doback/analysis/{vehicleId}/latest");
                return response ?? throw new Exception("No se pudo obtener el último análisis");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el último análisis para el vehículo {VehicleId}", vehicleId);
                throw;
            }
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

        public async Task<DobackAnalysis> GetAnalysisWithDataAsync(int vehicleId, DobackData data)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"api/doback/analysis/{vehicleId}", data);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<DobackAnalysis>();
                return result ?? throw new Exception("No se pudo obtener el análisis con los datos proporcionados");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el análisis con datos para el vehículo {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<DobackAnalysis> GetTrendAnalysisAsync(int vehicleId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<DobackAnalysis>($"api/doback/analysis/{vehicleId}/trends");
                return response ?? throw new Exception("No se pudo obtener el análisis de tendencias");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el análisis de tendencias para el vehículo {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<DobackAnalysis> GetPatternAnalysisAsync(int vehicleId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<DobackAnalysis>($"api/doback/analysis/{vehicleId}/patterns");
                return response ?? throw new Exception("No se pudo obtener el análisis de patrones");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el análisis de patrones para el vehículo {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<DobackAnalysis> GetAnomalyAnalysisAsync(int vehicleId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<DobackAnalysis>($"api/doback/analysis/{vehicleId}/anomalies");
                return response ?? throw new Exception("No se pudo obtener el análisis de anomalías");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el análisis de anomalías para el vehículo {VehicleId}", vehicleId);
                throw;
            }
        }
    }
} 