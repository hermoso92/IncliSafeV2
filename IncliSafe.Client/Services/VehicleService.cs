using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using IncliSafe.Client.Services.Interfaces;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Shared.Models.Core;
using IncliSafe.Shared.DTOs;
using Microsoft.Extensions.Logging;
using MudBlazor;
using IncliSafe.Shared.Models.Analysis.Core;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Analysis;

namespace IncliSafe.Client.Services
{
    public class VehicleService : ServiceBase, IVehicleService
    {
        private readonly ILogger<VehicleService> _logger;
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/vehicles";

        public VehicleService(ILogger<VehicleService> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<List<Vehicle>> GetVehiculosAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<Vehicle>>("api/vehicles");
                return response ?? new List<Vehicle>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de vehículos");
                throw;
            }
        }

        public async Task<Vehicle> GetVehiculoAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<Vehicle>($"api/vehicles/{id}");
                return response ?? throw new InvalidOperationException($"No se encontró el vehículo con ID {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el vehículo con ID {VehicleId}", id);
                throw;
            }
        }

        public async Task<Vehicle> CreateVehiculoAsync(VehiculoDTO vehiculo)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/vehicles", vehiculo);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Vehicle>() ?? throw new InvalidOperationException("No se pudo crear el vehículo");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el vehículo");
                throw;
            }
        }

        public async Task<Vehicle> UpdateVehiculoAsync(int id, VehiculoDTO vehiculo)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/vehicles/{id}", vehiculo);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Vehicle>() ?? throw new InvalidOperationException($"No se pudo actualizar el vehículo con ID {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el vehículo con ID {VehicleId}", id);
                throw;
            }
        }

        public async Task DeleteVehiculoAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/vehicles/{id}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el vehículo con ID {VehicleId}", id);
                throw;
            }
        }

        public async Task<List<AnalysisPrediction>> GetPredictionsAsync(int vehicleId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<AnalysisPrediction>>($"api/vehicles/{vehicleId}/predictions");
                return response ?? new List<AnalysisPrediction>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener predicciones para el vehículo {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<TrendAnalysis> GetTrendAnalysisAsync(int vehicleId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<TrendAnalysis>($"api/vehicles/{vehicleId}/trends?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
                return response ?? throw new InvalidOperationException($"No se encontró el análisis de tendencias para el vehículo {vehicleId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener análisis de tendencias para el vehículo {VehicleId}", vehicleId);
                throw;
            }
        }

        public async Task<List<Anomaly>> GetAnomaliesAsync(int vehicleId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<Anomaly>>($"api/vehicles/{vehicleId}/anomalies");
                return response ?? new List<Anomaly>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting anomalies for vehicle {VehicleId}", vehicleId);
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

        public async Task<TrendAnalysis> GetTrendAnalysisAsync(int vehicleId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<TrendAnalysis>($"{BaseUrl}/{vehicleId}/trends") 
                    ?? new TrendAnalysis();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting trend analysis for vehicle {Id}", vehicleId);
                throw;
            }
        }
    }
} 