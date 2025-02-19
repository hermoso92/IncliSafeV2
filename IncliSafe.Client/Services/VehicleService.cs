using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using IncliSafe.Client.Services.Interfaces;
using IncliSafe.Shared.Models.DTOs;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Notifications;
using Microsoft.Extensions.Logging;
using MudBlazor;
using IncliSafe.Shared.Models.Analysis.Core;

namespace IncliSafe.Client.Services
{
    public class VehicleService : ServiceBase, IVehicleService
    {
        private readonly HttpClient _httpClient;
        private new readonly ILogger<VehicleService> _logger;
        private const string BaseUrl = "api/vehiculos";

        public VehicleService(
            HttpClient httpClient,
            ISnackbar snackbar,
            ILogger<VehicleService> logger,
            CacheService cache) : base(httpClient, snackbar, logger, cache)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<Vehiculo>> GetVehiculos()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Vehiculo>>($"{BaseUrl}") ?? new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vehicles");
                throw;
            }
        }

        public async Task<VehiculoDTO> GetVehicleAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/{id}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<VehiculoDTO>() ?? new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vehicle {Id}", id);
                throw;
            }
        }

        public async Task<List<VehiculoDTO>> GetVehiclesAsync()
        {
            try
            {
                var vehicles = await _httpClient.GetFromJsonAsync<List<Vehiculo>>($"{BaseUrl}");
                return vehicles?.Select(v => v.ToDTO()).ToList() ?? new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vehicles");
                throw;
            }
        }

        public async Task<VehiculoDTO> CreateVehicleAsync(VehiculoDTO vehicle)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}", vehicle);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<VehiculoDTO>() ?? vehicle;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating vehicle");
                throw;
            }
        }

        public async Task<VehiculoDTO> UpdateVehicleAsync(VehiculoDTO vehicle)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{vehicle.Id}", vehicle);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<VehiculoDTO>() ?? vehicle;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating vehicle {Id}", vehicle.Id);
                throw;
            }
        }

        public async Task<bool> DeleteVehicleAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting vehicle {Id}", id);
                throw;
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/{id}/exists");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking vehicle existence {Id}", id);
                throw;
            }
        }

        public async Task<List<Alert>> GetAlertsAsync(int vehicleId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Alert>>($"{BaseUrl}/{vehicleId}/alerts") ?? new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting alerts for vehicle {Id}", vehicleId);
                throw;
            }
        }

        public async Task<NotificationSettings> GetNotificationSettingsAsync(int vehicleId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<NotificationSettings>($"{BaseUrl}/{vehicleId}/notifications/settings") 
                    ?? new NotificationSettings();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting notification settings for vehicle {Id}", vehicleId);
                throw;
            }
        }

        public async Task<List<Vehiculo>> GetUserVehiculos(int userId)
        {
            return await _httpClient.GetFromJsonAsync<List<Vehiculo>>($"api/vehiculos/user/{userId}") 
                ?? new List<Vehiculo>();
        }

        public async Task<List<Inspeccion>> GetInspeccionesAsync(int vehiculoId)
        {
            return await _httpClient.GetFromJsonAsync<List<Inspeccion>>($"api/vehiculos/{vehiculoId}/inspecciones") 
                ?? new List<Inspeccion>();
        }

        public async Task<Vehiculo?> GetVehicle(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<Vehiculo>($"{BaseUrl}/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vehicle {Id}", id);
                return null;
            }
        }

        public async Task<Vehiculo?> CreateVehiculo(Vehiculo vehiculo)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}", vehiculo);
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
                var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{vehiculo.Id}", vehiculo);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating vehicle {Id}", vehiculo.Id);
                throw;
            }
        }

        public async Task<bool> DeleteVehiculo(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting vehicle {Id}", id);
                throw;
            }
        }

        public async Task<List<DobackAnalysis>> GetAnalysesAsync(int vehicleId)
        {
            return await _httpClient.GetFromJsonAsync<List<DobackAnalysis>>($"{BaseUrl}/{vehicleId}/analyses") ?? new();
        }

        public async Task<TrendAnalysisEntity> GetTrendAnalysisAsync(int vehicleId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<TrendAnalysisEntity>($"{BaseUrl}/{vehicleId}/trends") 
                    ?? new TrendAnalysisEntity();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting trend analysis for vehicle {Id}", vehicleId);
                throw;
            }
        }

        public async Task<List<IncliSafe.Shared.Models.Analysis.Core.Prediction>> GetPredictionsAsync(int vehicleId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<IncliSafe.Shared.Models.Analysis.Core.Prediction>>($"{BaseUrl}/{vehicleId}/predictions") ?? new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting predictions for vehicle {Id}", vehicleId);
                throw;
            }
        }

        public async Task<List<Anomaly>> GetAnomaliesAsync(int vehicleId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Anomaly>>($"{BaseUrl}/{vehicleId}/anomalies") ?? new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting anomalies for vehicle {Id}", vehicleId);
                throw;
            }
        }
    }
} 