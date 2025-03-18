using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using IncliSafe.Client.Services.Interfaces;
using IncliSafe.Shared.Models.Auth;
using IncliSafe.Shared.Models.Entities;
using Microsoft.Extensions.Logging;
using MudBlazor;
using Blazored.LocalStorage;

namespace IncliSafe.Client.Services
{
    public class AuthenticationService : ServiceBase, IAuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private const string BaseUrl = "api/auth";
        private const string TokenKey = "authToken";
        private const string UserKey = "currentUser";

        public AuthenticationService(
            HttpClient httpClient,
            ISnackbar snackbar,
            ILogger<AuthenticationService> logger,
            ILocalStorageService localStorage,
            CacheService cache) : base(httpClient, snackbar, logger, cache)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<bool> Login(LoginRequest request)
        {
            var result = await HandleRequestAsync(async () =>
            {
                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/login", request);
                response.EnsureSuccessStatusCode();
                var authResult = await response.Content.ReadFromJsonAsync<AuthResponse>();
                
                if (authResult?.Token != null)
                {
                    await _localStorage.SetItemAsync(TokenKey, authResult.Token);
                    await _localStorage.SetItemAsync(UserKey, authResult.User);
                    return true;
                }
                return false;
            }, "Error al iniciar sesión");

            return result.Data;
        }

        public async Task<bool> Logout()
        {
            try
            {
                await _localStorage.RemoveItemAsync(TokenKey);
                await _localStorage.RemoveItemAsync(UserKey);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cerrar sesión");
                return false;
            }
        }

        public async Task<Usuario?> GetCurrentUser()
        {
            try
            {
                var user = await _localStorage.GetItemAsync<Usuario>(UserKey);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuario actual");
                return null;
            }
        }

        public async Task<bool> IsAuthenticated()
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>(TokenKey);
                return !string.IsNullOrEmpty(token);
            }
            catch
            {
                return false;
            }
        }
    }
} 