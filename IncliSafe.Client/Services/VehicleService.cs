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
    public class VehicleService : ServiceBase, IVehicleService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/vehiculos";

        public VehicleService(
            HttpClient httpClient,
            ISnackbar snackbar,
            ILogger<VehicleService> logger,
            CacheService cache) : base(httpClient, snackbar, logger, cache)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Vehiculo>> GetVehiculosAsync()
        {
            var result = await GetFromCacheOrApiAsync<List<Vehiculo>>(
                "vehiculos",
                BaseUrl,
                TimeSpan.FromMinutes(5));

            return result.Data ?? new List<Vehiculo>();
        }

        public async Task<Vehiculo> GetVehiculoAsync(int id)
        {
            var result = await HandleRequestAsync(async () =>
            {
                return await _httpClient.GetFromJsonAsync<Vehiculo>($"{BaseUrl}/{id}");
            }, "Error al obtener el vehículo");

            return result.Data ?? throw new Exception("Vehículo no encontrado");
        }

        public async Task<Vehiculo> CreateVehiculoAsync(Vehiculo vehiculo)
        {
            var result = await HandleRequestAsync(async () =>
            {
                var response = await _httpClient.PostAsJsonAsync(BaseUrl, vehiculo);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Vehiculo>();
            }, "Error al crear el vehículo");

            return result.Data ?? throw new Exception("Error al crear el vehículo");
        }

        public async Task<Vehiculo> UpdateVehiculoAsync(Vehiculo vehiculo)
        {
            var result = await HandleRequestAsync(async () =>
            {
                var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{vehiculo.Id}", vehiculo);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Vehiculo>();
            }, "Error al actualizar el vehículo");

            return result.Data ?? throw new Exception("Error al actualizar el vehículo");
        }

        public async Task DeleteVehiculoAsync(int id)
        {
            await HandleRequestAsync(async () =>
            {
                var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
                response.EnsureSuccessStatusCode();
                return true;
            }, "Error al eliminar el vehículo");
        }

        public async Task<List<Inspeccion>> GetInspeccionesAsync(int vehiculoId)
        {
            var result = await HandleRequestAsync(async () =>
            {
                return await _httpClient.GetFromJsonAsync<List<Inspeccion>>($"{BaseUrl}/{vehiculoId}/inspecciones");
            }, "Error al obtener inspecciones");

            return result.Data ?? new List<Inspeccion>();
        }
    }
} 