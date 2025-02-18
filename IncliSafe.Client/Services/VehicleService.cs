using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using IncliSafe.Client.Services.Interfaces;
using IncliSafe.Shared.Models.Entities;
using Microsoft.Extensions.Logging;
using MudBlazor;

namespace IncliSafe.Client.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<VehicleService> _logger;

        public VehicleService(HttpClient httpClient, ILogger<VehicleService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<Vehiculo>> GetVehiculos()
        {
            return await _httpClient.GetFromJsonAsync<List<Vehiculo>>("api/vehiculos") 
                ?? new List<Vehiculo>();
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
            var response = await _httpClient.PutAsJsonAsync($"api/vehiculos/{vehiculo.Id}", vehiculo);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteVehiculo(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/vehiculos/{id}");
            return response.IsSuccessStatusCode;
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
    }
} 