using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using IncliSafe.Shared.Models;
using IncliSafe.Client.Services;
using IncliSafe.Client.Services.Interfaces;
using MudBlazor;
using Microsoft.Extensions.Logging;
using IncliSafe.Shared.Models.Notifications;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Exceptions;
using IncliSafe.Shared.Models.DTOs;
using IncliSafe.Shared.Models.Analysis.Core;

namespace IncliSafe.Client.Services
{
    public class DobackAnalysisService : ServiceBase, IDobackAnalysisService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/doback";
        private new readonly ILogger<DobackAnalysisService> _logger;

        public DobackAnalysisService(
            HttpClient httpClient,
            ISnackbar snackbar,
            ILogger<DobackAnalysisService> logger,
            CacheService cache) : base(httpClient, snackbar, logger, cache)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<DobackAnalysis> AnalyzeFile(int vehicleId, Stream fileStream, string fileName)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(fileStream), "file", fileName);
            content.Add(new StringContent(vehicleId.ToString()), "vehicleId");

            var result = await HandleRequestAsync(async () =>
            {
                var response = await _httpClient.PostAsync($"{BaseUrl}/analyze", content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<DobackAnalysis>();
            }, "Error al analizar el archivo");

            return result.Data;
        }

        public async Task<List<DobackAnalysis>> GetVehicleAnalyses(int vehicleId)
        {
            return await _httpClient.GetFromJsonAsync<List<DobackAnalysis>>($"{BaseUrl}/vehicle/{vehicleId}/analyses")
                ?? new List<DobackAnalysis>();
        }

        public async Task<AnalysisResult> GetLatestAnalysis(int vehicleId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/vehicle/{vehicleId}/latest");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<AnalysisResult>() ?? new AnalysisResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting latest analysis");
                throw;
            }
        }

        public async Task<Dictionary<string, double>> GetTrends(int vehicleId)
        {
            return await _httpClient.GetFromJsonAsync<Dictionary<string, double>>($"{BaseUrl}/vehicle/{vehicleId}/trends")
                ?? new Dictionary<string, double>();
        }

        public async Task<List<IncliSafe.Shared.Models.Patterns.DetectedPattern>> GetDetectedPatterns(int analysisId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/analysis/{analysisId}/patterns");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<IncliSafe.Shared.Models.Patterns.DetectedPattern>>() ?? new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting detected patterns");
                throw;
            }
        }

        public async Task<DobackAnalysis?> GetAnalysisAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<DobackAnalysis>($"{BaseUrl}/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting analysis {Id}", id);
                return null;
            }
        }

        public async Task<List<DobackAnalysis>> GetAnalyses(int vehicleId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<DobackAnalysis>>($"{BaseUrl}/analysis/{vehicleId}") ?? new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting analyses for vehicle {Id}", vehicleId);
                return new();
            }
        }

        public async Task<DobackAnalysis> ProcessFile(int vehicleId, Stream fileStream)
        {
            var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(fileStream);
            content.Add(streamContent, "file", "doback.dat");

            var result = await HandleRequestAsync(async () =>
            {
                var response = await _httpClient.PostAsync($"{BaseUrl}/process/{vehicleId}", content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<DobackAnalysis>();
            }, "Error al procesar el archivo");

            return result.Data ?? throw new Exception("Error al procesar el archivo");
        }

        public async Task DeleteAnalysis(int fileId)
        {
            await HandleRequestAsync(async () =>
            {
                var response = await _httpClient.DeleteAsync($"{BaseUrl}/analysis/{fileId}");
                response.EnsureSuccessStatusCode();
                return true;
            }, "Error al eliminar el análisis");
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

        public async Task<List<DobackData>> GetDobackData(int fileId)
        {
            var result = await HandleRequestAsync(async () =>
            {
                return await _httpClient.GetFromJsonAsync<List<DobackData>>($"{BaseUrl}/data/{fileId}");
            }, "Error al obtener datos");

            return result.Data ?? new List<DobackData>();
        }

        public async Task<List<DobackData>> GetHistoricalData(int vehicleId, DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<DobackData>>($"{BaseUrl}/historical/{vehicleId}?start={startDate:s}&end={endDate:s}")
                    ?? new List<DobackData>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting historical data");
                throw;
            }
        }

        public async Task<DobackAnalysis> ProcessData(List<DobackData> data)
        {
            // Implementation needed
            throw new NotImplementedException();
        }

        public async Task<TrendData> GetTrendData(int vehicleId, DateTime start, DateTime end)
        {
            return await _httpClient.GetFromJsonAsync<TrendData>($"{BaseUrl}/trends/{vehicleId}?start={start:s}&end={end:s}")
                ?? new TrendData();
        }

        public async Task<List<PatternDetails>> GetPatternDetails(int patternId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/pattern/{patternId}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<PatternDetails>>()
                    ?? new List<PatternDetails>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pattern details");
                throw;
            }
        }

        public async Task<List<PatternHistory>> GetPatternHistory(int patternId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/doback/pattern/{patternId}/history");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<PatternHistory>>() ?? new List<PatternHistory>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pattern history");
                throw;
            }
        }

        public async Task<NotificationSettings> GetNotificationSettings(int vehicleId)
        {
            var response = await _httpClient.GetAsync($"api/notifications/settings/{vehicleId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<NotificationSettings>()
                ?? new NotificationSettings { VehicleId = vehicleId };
        }

        public async Task<List<DobackFileInfo>> GetVehicleFiles(int vehicleId)
        {
            return await _httpClient.GetFromJsonAsync<List<DobackFileInfo>>($"{BaseUrl}/files/{vehicleId}")
                ?? new List<DobackFileInfo>();
        }

        public async Task<bool> ExportAnalysis(int fileId, string format)
        {
            try
            {
                var response = await _httpClient.PostAsync($"{BaseUrl}/export/{fileId}?format={format}", null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting analysis");
                return false;
            }
        }

        public async Task<NotificationSettings> UpdateNotificationSettings(NotificationSettings settings)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/notifications/settings", settings);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<NotificationSettings>()
                ?? throw new InvalidOperationException("Failed to update notification settings");
        }

        public async Task<ICollection<DobackData>> GetDobackDataAsync(int analysisId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<DobackData>>($"{BaseUrl}/{analysisId}/data") ?? new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting doback data for analysis {Id}", analysisId);
                return new List<DobackData>();
            }
        }

        public async Task<List<AnalysisPrediction>> GetPredictions(int analysisId)
        {
            var response = await _httpClient.GetAsync($"api/doback/{analysisId}/predictions");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<AnalysisPrediction>>() ?? new();
            }
            return new();
        }

        public async Task<AlertSettings> GetAlertSettings(int patternId)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/alerts/settings/{patternId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<AlertSettings>()
                ?? new AlertSettings();
        }

        public async Task<List<DobackData>> GetData(int analysisId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/doback/data/{analysisId}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<DobackData>>() ?? new List<DobackData>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting doback data");
                throw;
            }
        }

        public async Task<TrendAnalysis> GetTrendAnalysis(int analysisId)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/analysis/{analysisId}/trends");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TrendAnalysis>()
                ?? new TrendAnalysis();
        }

        public async Task<IncliSafe.Shared.Models.Analysis.AnalysisPrediction> GetPrediction(int vehicleId)
        {
            // Implementation needed
            throw new NotImplementedException();
        }

        public async Task<TrendData[]> GetTrendDataAsync(int vehicleId)
        {
            return await _httpClient.GetFromJsonAsync<TrendData[]>($"{BaseUrl}/trends/{vehicleId}") 
                ?? Array.Empty<TrendData>();
        }

        public async Task<PatternDetails> GetPatternDetailsAsync(string patternId)
        {
            return await _httpClient.GetFromJsonAsync<PatternDetails>($"{BaseUrl}/patterns/{patternId}")
                ?? new PatternDetails();
        }

        public async Task<PatternHistory[]> GetPatternHistoryAsync(int vehicleId)
        {
            return await _httpClient.GetFromJsonAsync<PatternHistory[]>($"{BaseUrl}/history/{vehicleId}")
                ?? Array.Empty<PatternHistory>();
        }

        public async Task<AnalysisPrediction> GetPredictionAsync(int vehicleId)
        {
            return await _httpClient.GetFromJsonAsync<AnalysisPrediction>($"{BaseUrl}/predict/{vehicleId}")
                ?? new AnalysisPrediction();
        }

        public async Task<DashboardMetrics> GetDashboardMetrics()
        {
            return await _httpClient.GetFromJsonAsync<DashboardMetrics>($"{BaseUrl}/metrics")
                ?? new DashboardMetrics();
        }

        public async Task<bool> DeleteAnalysisAsync(int analysisId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{BaseUrl}/{analysisId}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting analysis {Id}", analysisId);
                return false;
            }
        }

        public async Task<DobackData> GetDobackDataAsync(int vehicleId)
        {
            return await _httpClient.GetFromJsonAsync<DobackData>($"{BaseUrl}/data/{vehicleId}")
                ?? new DobackData();
        }

        public async Task<List<AnalysisPrediction>> GetPredictionsAsync(int analysisId)
        {
            return await _httpClient.GetFromJsonAsync<List<AnalysisPrediction>>($"{BaseUrl}/{analysisId}/predictions")
                ?? new List<AnalysisPrediction>();
        }

        public async Task<AnalysisResult> AnalyzeFileAsync(int fileId)
        {
            var response = await _httpClient.PostAsync($"{BaseUrl}/analyze/{fileId}", null);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<AnalysisResult>()
                ?? new AnalysisResult();
        }

        public async Task<AnalysisPrediction> GeneratePredictionAsync(int vehicleId)
        {
            var response = await _httpClient.PostAsync($"{BaseUrl}/predict/{vehicleId}", null);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<AnalysisPrediction>()
                ?? new AnalysisPrediction();
        }

        public async Task<DashboardMetrics> GetDashboardMetricsAsync(int vehicleId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<DashboardMetrics>($"api/doback/dashboard/{vehicleId}");
                return response ?? new DashboardMetrics { VehicleId = vehicleId };
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
                var response = await _httpClient.GetFromJsonAsync<List<DobackAnalysis>>($"api/doback/history/{vehicleId}?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
                return response ?? new List<DobackAnalysis>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting analysis history for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<DobackAnalysis> CreateAnalysisAsync(int vehicleId, List<DobackData> data)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"api/doback/analyze/{vehicleId}", data);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<DobackAnalysis>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating analysis for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<List<PatternDetails>> GetDetectedPatternsAsync(int vehicleId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<PatternDetails>>($"api/doback/patterns/{vehicleId}?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
                return response ?? new List<PatternDetails>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting detected patterns for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<List<Alert>> GetRecentAlertsAsync(int vehicleId, int count = 10)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<Alert>>($"api/doback/alerts/{vehicleId}?count={count}");
                return response ?? new List<Alert>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting recent alerts for vehicle {VehicleId}", vehicleId);
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
                _logger.LogError(ex, "Error getting predictions for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<DobackAnalysis> GetAnalysisByIdAsync(int analysisId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<DobackAnalysis>($"api/doback/analysis/{analysisId}");
                return response ?? throw new InvalidOperationException($"Analysis with ID {analysisId} not found.");
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
                var response = await _httpClient.GetFromJsonAsync<TrendAnalysis>($"api/doback/trends/{vehicleId}?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
                return response ?? throw new InvalidOperationException($"Trend analysis not found for vehicle {vehicleId}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting trend analysis for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<DobackAnalysis> AnalyzeDobackAsync(int vehicleId, DobackAnalysisDTO analysisDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"api/doback/analyze/{vehicleId}", analysisDto);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<DobackAnalysis>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error analyzing doback for vehicle {VehicleId}", vehicleId);
                throw;
            }
        }
    }
} 