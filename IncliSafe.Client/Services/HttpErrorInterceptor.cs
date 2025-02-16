using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MudBlazor;
using System.Text.Json;

namespace IncliSafe.Client.Services
{
    public class HttpErrorInterceptor : DelegatingHandler
    {
        private readonly ISnackbar _snackbar;
        private readonly ILogger<HttpErrorInterceptor> _logger;

        public HttpErrorInterceptor(
            ISnackbar snackbar,
            ILogger<HttpErrorInterceptor> logger)
        {
            _snackbar = snackbar;
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await base.SendAsync(request, cancellationToken);

                if (!response.IsSuccessStatusCode && response.StatusCode != System.Net.HttpStatusCode.Unauthorized)
                {
                    var error = await response.Content.ReadAsStringAsync(cancellationToken);
                    _snackbar.Add($"Error: {error}", Severity.Error);
                    _logger.LogError("HTTP Error {StatusCode}: {Error}", response.StatusCode, error);
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la comunicación HTTP");
                _snackbar.Add("Error de conexión con el servidor", Severity.Error);
                throw;
            }
        }
    }
} 