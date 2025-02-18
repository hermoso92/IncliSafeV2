using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using IncliSafe.Shared.Models.DTOs;
using IncliSafe.Client.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using MudBlazor;
using IncliSafe.Shared.Exceptions;

namespace IncliSafe.Client.Services
{
    public class InspeccionService : ServiceBase, IInspeccionService
    {
        private readonly HttpClient _httpClient;
        private new readonly ILogger<InspeccionService> _logger;
        private new readonly ISnackbar _snackbar;

        public InspeccionService(
            HttpClient httpClient,
            ILogger<InspeccionService> logger,
            ISnackbar snackbar,
            CacheService cache) : base(httpClient, snackbar, logger, cache)
        {
            _httpClient = httpClient;
            _logger = logger;
            _snackbar = snackbar;
        }

        public async Task<List<InspeccionDTO>> GetInspeccionesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<InspeccionDTO>>("api/inspecciones") ?? new();
        }

        public async Task<InspeccionDTO> GetInspeccionAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<InspeccionDTO>($"api/inspecciones/{id}") 
                ?? throw new NotFoundException("Inspección no encontrada");
        }

        public async Task<InspeccionDTO> CreateInspeccionAsync(InspeccionDTO inspeccion)
        {
            var response = await _httpClient.PostAsJsonAsync("api/inspecciones", inspeccion);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<InspeccionDTO>() ?? throw new Exception("Error al crear la inspección");
        }

        public async Task<InspeccionDTO> UpdateInspeccionAsync(InspeccionDTO inspeccion)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/inspecciones/{inspeccion.Id}", inspeccion);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<InspeccionDTO>() ?? throw new Exception("Error al actualizar la inspección");
        }

        public async Task<bool> DeleteInspeccionAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/inspecciones/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
