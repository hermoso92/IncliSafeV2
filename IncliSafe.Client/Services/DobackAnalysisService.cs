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

namespace IncliSafe.Client.Services
{
    public class DobackAnalysisService : ServiceBase, IDobackAnalysisService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/doback";

        public DobackAnalysisService(
            HttpClient httpClient,
            ISnackbar snackbar,
            ILogger<DobackAnalysisService> logger,
            CacheService cache) : base(httpClient, snackbar, logger, cache)
        {
            _httpClient = httpClient;
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
            return await _httpClient.GetFromJsonAsync<AnalysisResult>($"{BaseUrl}/vehicle/{vehicleId}/latest")
                ?? new AnalysisResult();
        }

        public async Task<Dictionary<string, double>> GetTrends(int vehicleId)
        {
            return await _httpClient.GetFromJsonAsync<Dictionary<string, double>>($"{BaseUrl}/vehicle/{vehicleId}/trends")
                ?? new Dictionary<string, double>();
        }

        public async Task<List<DetectedPattern>> GetDetectedPatterns(int vehicleId)
        {
            return await _httpClient.GetFromJsonAsync<List<DetectedPattern>>($"{BaseUrl}/vehicle/{vehicleId}/patterns")
                ?? new List<DetectedPattern>();
        }

        public async Task<DobackAnalysis> GetAnalysis(int fileId)
        {
            var result = await HandleRequestAsync(async () =>
            {
                return await _httpClient.GetFromJsonAsync<DobackAnalysis>($"{BaseUrl}/analysis/{fileId}");
            }, "Error al obtener el análisis");

            return result.Data ?? throw new Exception("Análisis no encontrado");
        }

        public async Task<List<DobackAnalysis>> GetAnalyses(int vehicleId)
        {
            var result = await GetFromCacheOrApiAsync<List<DobackAnalysis>>(
                $"analyses_{vehicleId}",
                $"{BaseUrl}/vehicle/{vehicleId}",
                TimeSpan.FromMinutes(5));

            return result.Data ?? new List<DobackAnalysis>();
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
    }
} 