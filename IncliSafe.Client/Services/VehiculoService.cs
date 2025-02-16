using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using IncliSafe.Shared.Models;
using MudBlazor;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components.Authorization;
using System.Text.Json;

namespace IncliSafe.Client.Services
{
    public class VehiculoService : ServiceBase
    {
        public VehiculoService(
            HttpClient http,
            ISnackbar snackbar,
            ILogger<VehiculoService> logger,
            CacheService cache)
            : base(http, snackbar, logger, cache)
        {
        }

        public async Task<List<Vehiculo>> GetVehiculos()
        {
            try
            {
                _logger.LogInformation("Obteniendo vehículos...");
                var response = await _http.GetAsync("api/vehiculos");
                
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Error al obtener vehículos: {error}");
                    _snackbar.Add($"Error: {error}", Severity.Error);
                    return new List<Vehiculo>();
                }

                var vehiculos = await response.Content.ReadFromJsonAsync<List<Vehiculo>>();
                _logger.LogInformation($"Vehículos obtenidos: {vehiculos?.Count ?? 0}");
                return vehiculos ?? new List<Vehiculo>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener vehículos");
                _snackbar.Add($"Error: {ex.Message}", Severity.Error);
                return new List<Vehiculo>();
            }
        }

        public async Task<Vehiculo?> GetVehiculo(int id)
        {
            try
            {
                var response = await _http.GetAsync($"api/vehiculos/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Vehiculo>();
                }
                
                var error = await response.Content.ReadAsStringAsync();
                _snackbar.Add(error, Severity.Error);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener vehículo");
                _snackbar.Add($"Error al obtener vehículo: {ex.Message}", Severity.Error);
                return null;
            }
        }

        public async Task<Vehiculo?> CreateVehiculo(Vehiculo vehiculo)
        {
            try
            {
                _logger.LogInformation($"[CreateVehiculo] Iniciando creación de vehículo: {JsonSerializer.Serialize(vehiculo)}");
                
                var authState = await _authStateProvider.GetAuthenticationStateAsync();
                var userId = authState.User.FindFirst("UserId")?.Value;
                
                if (string.IsNullOrEmpty(userId))
                {
                    _snackbar.Add("Error: Usuario no identificado", Severity.Error);
                    return null;
                }

                vehiculo.UserId = int.Parse(userId);
                
                var response = await _http.PostAsJsonAsync("api/vehiculos", vehiculo);
                
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _snackbar.Add($"Error: {error}", Severity.Error);
                    return null;
                }

                var nuevoVehiculo = await response.Content.ReadFromJsonAsync<Vehiculo>();
                if (nuevoVehiculo == null)
                {
                    _snackbar.Add("Error al procesar la respuesta del servidor", Severity.Error);
                    return null;
                }

                _snackbar.Add("Vehículo creado correctamente", Severity.Success);
                return nuevoVehiculo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear vehículo");
                _snackbar.Add($"Error: {ex.Message}", Severity.Error);
                return null;
            }
        }

        public async Task<Vehiculo?> UpdateVehiculo(Vehiculo vehiculo)
        {
            try
            {
                var response = await _http.PutAsJsonAsync($"api/vehiculos/{vehiculo.Id}", vehiculo);
                
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _snackbar.Add($"Error: {error}", Severity.Error);
                    return null;
                }

                _snackbar.Add("Vehículo actualizado correctamente", Severity.Success);
                return vehiculo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar vehículo");
                _snackbar.Add($"Error: {ex.Message}", Severity.Error);
                return null;
            }
        }

        public async Task DeleteVehiculo(int id)
        {
            try
            {
                var response = await _http.DeleteAsync($"api/vehiculos/{id}");
                response.EnsureSuccessStatusCode();
                _snackbar.Add("Vehículo eliminado correctamente", Severity.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar vehículo");
                _snackbar.Add($"Error: {ex.Message}", Severity.Error);
                throw;
            }
        }
    }
}
