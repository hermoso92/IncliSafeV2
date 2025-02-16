using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using IncliSafe.Shared.Models;
using Microsoft.Extensions.Logging;

namespace IncliSafe.Client.Services
{
    public class AuthenticationHttpMessageHandler : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorage;
        private readonly NavigationManager _navigationManager;
        private readonly ISnackbar _snackbar;
        private readonly ILogger<AuthenticationHttpMessageHandler> _logger;

        public AuthenticationHttpMessageHandler(
            ILocalStorageService localStorage,
            NavigationManager navigationManager,
            ISnackbar snackbar,
            ILogger<AuthenticationHttpMessageHandler> logger)
        {
            _localStorage = localStorage;
            _navigationManager = navigationManager;
            _snackbar = snackbar;
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                var userSession = await _localStorage.GetItemAsync<UserSession>("UserSession");
                if (userSession != null)
                {
                    request.Headers.Authorization = 
                        new AuthenticationHeaderValue("Bearer", userSession.Token);
                }

                var response = await base.SendAsync(request, cancellationToken);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await _localStorage.RemoveItemAsync("UserSession");
                    _snackbar.Add("Su sesión ha expirado", Severity.Warning);
                    _navigationManager.NavigateTo("/login", true);
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la comunicación con el servidor");
                throw;
            }
        }
    }
} 