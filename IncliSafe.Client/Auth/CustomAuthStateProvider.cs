using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using Microsoft.Extensions.Logging;
using IncliSafe.Shared.Models.Auth;
using System.Collections.Generic;

namespace IncliSafe.Client.Auth
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationState _anonymous;
        private readonly HttpClient _http;

        public CustomAuthStateProvider(
            ILocalStorageService localStorage,
            HttpClient http)
        {
            _localStorage = localStorage;
            _http = http;
            _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var userSession = await _localStorage.GetItemAsync<UserSession>("UserSession");
            if (userSession == null)
                return _anonymous;

            var claimsPrincipal = GetClaimsPrincipal(userSession);
            return new AuthenticationState(claimsPrincipal);
        }

        public async Task UpdateAuthenticationState(UserSession? userSession)
        {
            if (userSession != null)
            {
                await _localStorage.SetItemAsync("UserSession", userSession);
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            }
            else
            {
                await _localStorage.RemoveItemAsync("UserSession");
                NotifyAuthenticationStateChanged(Task.FromResult(_anonymous));
            }
        }

        public async Task MarkUserAsAuthenticated(UserSession user)
        {
            await _localStorage.SetItemAsync("user", user);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task MarkUserAsLoggedOut()
        {
            await _localStorage.RemoveItemAsync("user");
            await _localStorage.RemoveItemAsync("authToken");
            _http.DefaultRequestHeaders.Authorization = null;
            NotifyAuthenticationStateChanged(Task.FromResult(_anonymous));
        }

        private ClaimsPrincipal GetClaimsPrincipal(UserSession userSession)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userSession.Name),
                new Claim(ClaimTypes.Role, userSession.Role),
                new Claim(ClaimTypes.NameIdentifier, userSession.Id.ToString()),
                new Claim("IsActive", userSession.IsActive.ToString())
            };

            var identity = new ClaimsIdentity(claims, "JwtAuth");
            return new ClaimsPrincipal(identity);
        }
    }
} 