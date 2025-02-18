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

namespace IncliSafe.Client.Services
{
    public class VehiculoService : ServiceBase, IVehicleService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authStateProvider;
        private new readonly ILogger<VehiculoService> _logger;

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
    }
}
