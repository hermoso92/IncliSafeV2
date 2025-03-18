using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using IncliSafe.Shared.Models;
using MudBlazor;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Client.Services.Interfaces;
using IncliSafe.Shared.Models.DTOs;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Shared.Models.Notifications;
using Anomaly = IncliSafe.Shared.Models.Analysis.Core.Anomaly;

namespace IncliSafe.Client.Services
{
    public class VehiculoService : ServiceBase, IVehicleService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authStateProvider;
        private new readonly ILogger<VehiculoService> _logger;
        private const string BaseUrl = "api/vehiculos";

        public VehiculoService(
            HttpClient httpClient,
            AuthenticationStateProvider authStateProvider,
            ILogger<VehiculoService> logger,
            ISnackbar snackbar,
            CacheService cache) : base(httpClient, snackbar, logger, cache)
        {
            _httpClient = httpClient;
            _authStateProvider = authStateProvider;
            _logger = logger;
        }

        public async Task<List<Vehiculo>> GetVehiculos()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Vehiculo>>("api/vehiculos") ?? new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vehicles");
                return new();
            }
        }

        public async Task<Vehiculo?> GetVehicle(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<Vehiculo>($"api/vehiculos/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vehicle {Id}", id);
                return null;
            }
        }

        public async Task<List<Vehiculo>> GetUserVehiculos(int userId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Vehiculo>>($"api/vehiculos/user/{userId}") ?? new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user vehicles");
                return new();
            }
        }

        public async Task<Vehiculo?> CreateVehiculo(Vehiculo vehiculo)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/vehiculos", vehiculo);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Vehiculo>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating vehicle");
                return null;
            }
        }

        public async Task<bool> UpdateVehiculo(Vehiculo vehiculo)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/vehiculos/{vehiculo.Id}", vehiculo);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating vehicle");
                return false;
            }
        }

        public async Task<bool> DeleteVehiculo(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/vehiculos/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting vehicle");
                return false;
            }
        }

        public async Task<List<Inspeccion>> GetInspeccionesAsync(int vehiculoId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Inspeccion>>($"api/vehiculos/{vehiculoId}/inspecciones") ?? new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting inspections");
                return new();
            }
        }

        public async Task<VehiculoDTO> GetVehicleAsync(int id)
        {
            // Implementar
            throw new NotImplementedException();
        }

        public async Task<List<VehiculoDTO>> GetVehiclesAsync()
        {
            // Implementar
            throw new NotImplementedException();
        }

        public async Task<VehiculoDTO> CreateVehicleAsync(VehiculoDTO vehicle)
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}", vehicle);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<VehiculoDTO>() ?? vehicle;
        }

        public async Task<VehiculoDTO> UpdateVehicleAsync(VehiculoDTO vehicle)
        {
            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{vehicle.Id}", vehicle);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<VehiculoDTO>() ?? vehicle;
        }

        public async Task<bool> DeleteVehicleAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/{id}/exists");
            return await response.Content.ReadFromJsonAsync<bool>();
        }

        public async Task<List<Alert>> GetAlertsAsync(int vehicleId)
        {
            return await _httpClient.GetFromJsonAsync<List<Alert>>($"{BaseUrl}/{vehicleId}/alerts") ?? new();
        }

        public async Task<NotificationSettings> GetNotificationSettingsAsync(int vehicleId)
        {
            return await _httpClient.GetFromJsonAsync<NotificationSettings>($"{BaseUrl}/{vehicleId}/notifications") ?? new();
        }

        public async Task<List<DobackAnalysis>> GetAnalysesAsync(int vehicleId)
        {
            return await _httpClient.GetFromJsonAsync<List<DobackAnalysis>>($"{BaseUrl}/{vehicleId}/analyses") ?? new();
        }

        public async Task<TrendAnalysis> GetTrendAnalysisAsync(int vehicleId)
        {
            return await _httpClient.GetFromJsonAsync<TrendAnalysis>($"{BaseUrl}/{vehicleId}/trends") ?? new();
        }

        public async Task<List<AnalysisPrediction>> GetPredictionsAsync(int vehicleId)
        {
            return await _httpClient.GetFromJsonAsync<List<AnalysisPrediction>>($"{BaseUrl}/{vehicleId}/predictions") ?? new();
        }

        public async Task<List<Anomaly>> GetAnomaliesAsync(int vehicleId)
        {
            return await _httpClient.GetFromJsonAsync<List<Anomaly>>($"{BaseUrl}/{vehicleId}/anomalies") ?? new();
        }
    }
}
