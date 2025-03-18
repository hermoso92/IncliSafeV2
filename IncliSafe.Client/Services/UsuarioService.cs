using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using IncliSafe.Shared.Models;
using Microsoft.Extensions.Logging;
using MudBlazor;

namespace IncliSafe.Client.Services
{
    public class UsuarioService
    {
        private readonly HttpClient _http;
        private readonly ILogger<UsuarioService> _logger;
        private readonly ISnackbar _snackbar;

        public UsuarioService(HttpClient http, ILogger<UsuarioService> logger, ISnackbar snackbar)
        {
            _http = http;
            _logger = logger;
            _snackbar = snackbar;
        }

        public async Task<List<Usuario>> GetUsuarios()
        {
            try
            {
                _logger.LogInformation("[GetUsuarios] Obteniendo usuarios...");
                var response = await _http.GetAsync("api/usuarios");
                
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"[GetUsuarios] Error: {error}");
                    _snackbar.Add($"Error al obtener usuarios: {error}", Severity.Error);
                    return new List<Usuario>();
                }

                var usuarios = await response.Content.ReadFromJsonAsync<List<Usuario>>();
                _logger.LogInformation($"[GetUsuarios] Usuarios obtenidos: {usuarios?.Count ?? 0}");
                return usuarios ?? new List<Usuario>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[GetUsuarios] Error al obtener usuarios");
                _snackbar.Add($"Error: {ex.Message}", Severity.Error);
                return new List<Usuario>();
            }
        }

        public async Task<Usuario?> CreateUsuario(Usuario usuario)
        {
            try
            {
                _logger.LogInformation($"[CreateUsuario] Creando usuario: {usuario.Nombre}");
                var response = await _http.PostAsJsonAsync("api/usuarios", usuario);
                
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"[CreateUsuario] Error: {error}");
                    _snackbar.Add($"Error: {error}", Severity.Error);
                    return null;
                }

                var nuevoUsuario = await response.Content.ReadFromJsonAsync<Usuario>();
                _logger.LogInformation($"[CreateUsuario] Usuario creado con ID: {nuevoUsuario?.Id}");
                return nuevoUsuario;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[CreateUsuario] Error al crear usuario");
                _snackbar.Add($"Error: {ex.Message}", Severity.Error);
                return null;
            }
        }

        public async Task<Usuario?> UpdateUsuario(Usuario usuario)
        {
            try
            {
                _logger.LogInformation($"[UpdateUsuario] Actualizando usuario: {usuario.Id}");
                var response = await _http.PutAsJsonAsync($"api/usuarios/{usuario.Id}", usuario);
                
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"[UpdateUsuario] Error: {error}");
                    _snackbar.Add($"Error: {error}", Severity.Error);
                    return null;
                }

                _logger.LogInformation($"[UpdateUsuario] Usuario actualizado: {usuario.Id}");
                return usuario;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[UpdateUsuario] Error al actualizar usuario");
                _snackbar.Add($"Error: {ex.Message}", Severity.Error);
                return null;
            }
        }

        public async Task DeleteUsuario(int id)
        {
            try
            {
                _logger.LogInformation($"Eliminando usuario ID: {id}");
                var response = await _http.DeleteAsync($"api/usuarios/{id}");
                response.EnsureSuccessStatusCode();
                _logger.LogInformation($"Usuario eliminado ID: {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar usuario");
                throw;
            }
        }
    }
}
