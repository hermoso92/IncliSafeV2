using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MudBlazor;

namespace IncliSafe.Client.Services
{
    public abstract class ServiceBase
    {
        protected readonly HttpClient _http;
        protected readonly ISnackbar _snackbar;
        protected readonly ILogger<ServiceBase> _logger;
        protected readonly CacheService _cache;

        protected ServiceBase(
            HttpClient http,
            ISnackbar snackbar,
            ILogger<ServiceBase> logger,
            CacheService cache)
        {
            _http = http;
            _snackbar = snackbar;
            _logger = logger;
            _cache = cache;
        }

        protected async Task<ServiceResult<T>> HandleRequestAsync<T>(
            Func<Task<T>> request, 
            string errorMessage,
            bool showSuccessMessage = false,
            string? successMessage = null)
        {
            try
            {
                var result = await request();
                
                if (showSuccessMessage)
                {
                    _snackbar.Add(successMessage ?? "Operación exitosa", Severity.Success);
                }
                
                return ServiceResult<T>.CreateSuccess(result);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _logger.LogWarning(ex, "Sesión expirada");
                return ServiceResult<T>.CreateError("Sesión expirada");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, errorMessage);
                _snackbar.Add($"Error de conexión: {ex.Message}", Severity.Error);
                return ServiceResult<T>.CreateError(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, errorMessage);
                _snackbar.Add($"Error: {ex.Message}", Severity.Error);
                return ServiceResult<T>.CreateError(ex.Message);
            }
        }

        protected async Task<ServiceResult<T>> GetFromCacheOrApiAsync<T>(
            string cacheKey, 
            string apiEndpoint,
            TimeSpan? cacheExpiration = null)
        {
            try
            {
                var result = await _cache.GetOrSetAsync(cacheKey, 
                    async () => await _http.GetFromJsonAsync<T>(apiEndpoint),
                    cacheExpiration);

                return result != null 
                    ? ServiceResult<T>.CreateSuccess(result) 
                    : ServiceResult<T>.CreateError("No se pudo obtener los datos");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener datos de {Endpoint}", apiEndpoint);
                return ServiceResult<T>.CreateError(ex.Message);
            }
        }

        protected async Task ClearCacheAsync(string key)
        {
            await _cache.ClearAsync(key);
        }
    }

    public class ServiceResult<T>
    {
        public required string Message { get; set; }
        public required T Data { get; set; }
        public bool Success { get; set; }

        private ServiceResult() { }  // Constructor privado para forzar el uso de los métodos factory

        public static ServiceResult<T> CreateSuccess(T data, string? message = null) =>
            new() { Success = true, Data = data, Message = message ?? string.Empty };

        public static ServiceResult<T> CreateError(string message) =>
            new() { Success = false, Message = message, Data = default! };
    }
} 