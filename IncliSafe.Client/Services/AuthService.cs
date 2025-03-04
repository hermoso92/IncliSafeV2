using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using Microsoft.Extensions.Logging;
using MudBlazor;
using IncliSafe.Client.Auth;
using IncliSafe.Client.Services.Interfaces;
using IncliSafe.Shared.Models.Auth;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafe.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            HttpClient http,
            ILocalStorageService localStorage,
            AuthenticationStateProvider authStateProvider,
            ILogger<AuthService> logger)
        {
            _http = http;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
            _logger = logger;
        }

        public async Task<UserSession> Login(string username, string password)
        {
            var request = new LoginRequest { Username = username, Password = password };
            var response = await _http.PostAsJsonAsync("api/auth/login", request);
            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
            
            if (result?.Success == true && result.User != null)
            {
                await _localStorage.SetItemAsync("authToken", result.Token);
                await ((CustomAuthStateProvider)_authStateProvider).MarkUserAsAuthenticated(result.User);
                return result.User;
            }
            
            throw new Exception(result?.Message ?? "Error en la autenticación");
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            await ((CustomAuthStateProvider)_authStateProvider).MarkUserAsLoggedOut();
        }

        public async Task<UserSession?> GetCurrentUser()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (string.IsNullOrEmpty(token))
                return null;

            var response = await _http.GetAsync("api/auth/user");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<UserSession>();
        }

        public async Task<bool> IsAuthenticated()
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>("authToken");
                if (string.IsNullOrEmpty(token))
                    return false;

                var user = await _localStorage.GetItemAsync<UserSession>("user");
                return user != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar autenticación");
                return false;
            }
        }
    }
} 