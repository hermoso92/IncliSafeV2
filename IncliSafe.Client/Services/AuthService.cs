using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using IncliSafe.Shared.Models;
using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using MudBlazor;
using IncliSafe.Client.Auth;
using IncliSafe.Client.Services.Interfaces;

namespace IncliSafe.Client.Services
{
    public class AuthService : ServiceBase, IAuthService
    {
        private readonly HttpClient _http;
        private readonly CustomAuthStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;
        private readonly ILogger<AuthService> _logger;
        private readonly ISnackbar _snackbar;

        public AuthService(
            HttpClient http,
            AuthenticationStateProvider authStateProvider,
            ILocalStorageService localStorage,
            ILogger<AuthService> logger,
            ISnackbar snackbar,
            CacheService cache) : base(http, snackbar, logger, cache)
        {
            _http = http;
            _authStateProvider = (CustomAuthStateProvider)authStateProvider;
            _localStorage = localStorage;
        }

        public async Task<UserSession> Login(string username, string password)
        {
            var request = new LoginRequest { Nombre = username, Password = password };
            var result = await HandleRequestAsync(async () =>
            {
                var response = await _http.PostAsJsonAsync("api/auth/login", request);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<UserSession>();
            }, "Error al iniciar sesión");

            if (result.Data != null)
            {
                await _localStorage.SetItemAsync("UserSession", result.Data);
                await _authStateProvider.MarkUserAsAuthenticated(result.Data);
            }

            return result.Data ?? throw new Exception("Error al iniciar sesión");
        }

        public async Task<UserSession?> GetCurrentUser()
        {
            return await _localStorage.GetItemAsync<UserSession>("UserSession");
        }

        public async Task Logout()
        {
            try
            {
                _logger.LogInformation("Iniciando proceso de logout...");
                await _localStorage.RemoveItemAsync("UserSession");
                _http.DefaultRequestHeaders.Authorization = null;
                await _authStateProvider.MarkUserAsLoggedOut();
                _snackbar.Add("Sesión cerrada correctamente", Severity.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en proceso de logout");
                _snackbar.Add($"Error al cerrar sesión: {ex.Message}", Severity.Error);
            }
        }

        public async Task<bool> IsAuthenticated()
        {
            try
            {
                var userSession = await _localStorage.GetItemAsync<UserSession>("UserSession");
                if (userSession != null)
                {
                    // Restaurar el token si existe la sesión
                    _http.DefaultRequestHeaders.Authorization = 
                        new AuthenticationHeaderValue("Bearer", userSession.Token);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar autenticación");
                return false;
            }
        }
    }
} 