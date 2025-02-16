using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using IncliSafe.Shared.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using MudBlazor;

namespace IncliSafe.Client.Services
{
    public class InspeccionService
    {
        private readonly HttpClient _http;
        private readonly ILogger<InspeccionService> _logger;
        private readonly ISnackbar _snackbar;

        public InspeccionService(HttpClient http, ILogger<InspeccionService> logger, ISnackbar snackbar)
        {
            _http = http;
            _logger = logger;
            _snackbar = snackbar;
        }

        public async Task<List<Inspeccion>> GetInspecciones()
        {
            try
            {
                var response = await _http.GetAsync("api/inspecciones");
                
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _snackbar.Add($"Error: {error}", Severity.Error);
                    return new List<Inspeccion>();
                }

                var inspecciones = await response.Content.ReadFromJsonAsync<List<Inspeccion>>();
                return inspecciones ?? new List<Inspeccion>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener inspecciones");
                _snackbar.Add($"Error: {ex.Message}", Severity.Error);
                return new List<Inspeccion>();
            }
        }

        public async Task<Inspeccion?> CreateInspeccion(Inspeccion inspeccion)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/inspecciones", inspeccion);
                
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _snackbar.Add($"Error: {error}", Severity.Error);
                    return null;
                }

                var nuevaInspeccion = await response.Content.ReadFromJsonAsync<Inspeccion>();
                _snackbar.Add("Inspección creada correctamente", Severity.Success);
                return nuevaInspeccion;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear inspección");
                _snackbar.Add($"Error: {ex.Message}", Severity.Error);
                return null;
            }
        }

        public async Task<Inspeccion?> UpdateInspeccion(Inspeccion inspeccion)
        {
            try
            {
                var response = await _http.PutAsJsonAsync($"api/inspecciones/{inspeccion.Id}", inspeccion);
                
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _snackbar.Add($"Error: {error}", Severity.Error);
                    return null;
                }

                _snackbar.Add("Inspección actualizada correctamente", Severity.Success);
                return inspeccion;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar inspección");
                _snackbar.Add($"Error: {ex.Message}", Severity.Error);
                return null;
            }
        }

        public async Task DeleteInspeccion(int id)
        {
            try
            {
                var response = await _http.DeleteAsync($"api/inspecciones/{id}");
                response.EnsureSuccessStatusCode();
                _snackbar.Add("Inspección eliminada correctamente", Severity.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar inspección");
                _snackbar.Add($"Error: {ex.Message}", Severity.Error);
                throw;
            }
        }
    }
}
