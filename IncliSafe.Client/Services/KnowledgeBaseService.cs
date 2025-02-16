using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using IncliSafe.Shared.Models.Knowledge;
using IncliSafe.Shared.Models.Patterns;
using IncliSafe.Client.Services.Interfaces;
using Microsoft.Extensions.Logging;
using MudBlazor;

namespace IncliSafe.Client.Services
{
    public class KnowledgeBaseService : ServiceBase, IKnowledgeBaseService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/knowledge";

        public KnowledgeBaseService(
            HttpClient httpClient,
            ISnackbar snackbar,
            ILogger<KnowledgeBaseService> logger,
            CacheService cache) : base(httpClient, snackbar, logger, cache)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> ProcessDataAsync(string data)
        {
            var result = await HandleRequestAsync(async () =>
            {
                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/process", data);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<bool>();
            }, "Error al procesar datos");

            return result.Data;
        }

        public async Task<List<KnowledgePattern>> GetPatterns()
        {
            var result = await GetFromCacheOrApiAsync<List<KnowledgePattern>>(
                "patterns",
                $"{BaseUrl}/patterns",
                TimeSpan.FromMinutes(5));

            return result.Data ?? new List<KnowledgePattern>();
        }

        public async Task<KnowledgePattern> GetPattern(int id)
        {
            var result = await HandleRequestAsync(async () =>
            {
                return await _httpClient.GetFromJsonAsync<KnowledgePattern>($"{BaseUrl}/patterns/{id}");
            }, "Error al obtener el patrón");

            return result.Data ?? throw new Exception("Patrón no encontrado");
        }

        public async Task<KnowledgePattern> CreatePattern(KnowledgePattern pattern)
        {
            var result = await HandleRequestAsync(async () =>
            {
                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/patterns", pattern);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<KnowledgePattern>();
            }, "Error al crear el patrón");

            return result.Data ?? throw new Exception("Error al crear el patrón");
        }

        public async Task<KnowledgePattern> UpdatePattern(KnowledgePattern pattern)
        {
            var result = await HandleRequestAsync(async () =>
            {
                var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/patterns/{pattern.Id}", pattern);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<KnowledgePattern>();
            }, "Error al actualizar el patrón");

            return result.Data ?? throw new Exception("Error al actualizar el patrón");
        }

        public async Task DeletePattern(int id)
        {
            await HandleRequestAsync(async () =>
            {
                var response = await _httpClient.DeleteAsync($"{BaseUrl}/patterns/{id}");
                response.EnsureSuccessStatusCode();
                return true;
            }, "Error al eliminar el patrón");
        }

        public async Task<KnowledgeStats> GetStats()
        {
            var result = await HandleRequestAsync(async () =>
            {
                return await _httpClient.GetFromJsonAsync<KnowledgeStats>($"{BaseUrl}/stats");
            }, "Error al obtener estadísticas");

            return result.Data ?? new KnowledgeStats();
        }

        public async Task<Dictionary<string, double>> GetPatternDistribution()
        {
            var result = await HandleRequestAsync(async () =>
            {
                return await _httpClient.GetFromJsonAsync<Dictionary<string, double>>($"{BaseUrl}/distribution");
            }, "Error al obtener distribución");

            return result.Data ?? new Dictionary<string, double>();
        }

        public async Task<List<PatternDetection>> GetPatternDetections(int patternId)
        {
            var result = await HandleRequestAsync(async () =>
            {
                return await _httpClient.GetFromJsonAsync<List<PatternDetection>>($"{BaseUrl}/patterns/{patternId}/detections");
            }, "Error al obtener detecciones");

            return result.Data ?? new List<PatternDetection>();
        }

        public async Task<PatternDetection> DetectPatternAsync(int patternId)
        {
            var result = await HandleRequestAsync(async () =>
            {
                return await _httpClient.GetFromJsonAsync<PatternDetection>($"{BaseUrl}/patterns/{patternId}/detect");
            }, "Error al detectar patrón");

            return result.Data ?? throw new Exception("Error al detectar patrón");
        }
    }
} 