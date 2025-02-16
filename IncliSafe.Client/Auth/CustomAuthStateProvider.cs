using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using Microsoft.Extensions.Logging;
using IncliSafe.Shared.Models;

namespace IncliSafe.Client.Auth
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationState _anonymous;
        private readonly HttpClient _httpClient;

        public CustomAuthStateProvider(
            ILocalStorageService localStorage,
            HttpClient httpClient)
        {
            _localStorage = localStorage;
            _httpClient = httpClient;
            _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userSession = await _localStorage.GetItemAsync<UserSession>("UserSession");
                if (userSession == null)
                {
                    return _anonymous;
                }

                // Restaurar el token en el HttpClient
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", userSession.Token);

                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, userSession.Nombre),
                    new Claim(ClaimTypes.Email, userSession.Email),
                    new Claim(ClaimTypes.Role, userSession.Rol),
                    new Claim("UserId", userSession.Id.ToString()),
                    new Claim("Token", userSession.Token)
                }, "JwtAuth"));

                return new AuthenticationState(claimsPrincipal);
            }
            catch
            {
                return _anonymous;
            }
        }

        public async Task MarkUserAsAuthenticated(UserSession userSession)
        {
            await _localStorage.SetItemAsync("UserSession", userSession);
            var authState = Task.FromResult(await GetAuthenticationStateAsync());
            NotifyAuthenticationStateChanged(authState);
        }

        public async Task MarkUserAsLoggedOut()
        {
            await _localStorage.RemoveItemAsync("UserSession");
            _httpClient.DefaultRequestHeaders.Authorization = null;
            var authState = Task.FromResult(_anonymous);
            NotifyAuthenticationStateChanged(authState);
        }
    }
} 