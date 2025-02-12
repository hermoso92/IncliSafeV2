using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace IncliSafe.Cliente.Services
{
    public class HttpService
    {
        private readonly HttpClient _http;
        private readonly string _baseUrl = "http://0.0.0.0:80/api";

        public HttpService(HttpClient http)
        {
            _http = http ?? throw new ArgumentNullException(nameof(http));
        }

        public async Task<T> GetAsync<T>(string url)
        {
            try
            {
                var response = await _http.GetFromJsonAsync<T>($"{_baseUrl}/{url}");
                return response ?? throw new Exception("No se recibieron datos del servidor");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error de conexión: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener datos: {ex.Message}");
            }
        }

        public async Task<T> PostAsync<T>(string url, T data)
        {
            try
            {
                var response = await _http.PostAsJsonAsync($"{_baseUrl}/{url}", data);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<T>();
            }
            catch (Exception)
            {
                throw new Exception("Error al enviar datos al servidor");
            }
        }

        public async Task<T> PutAsync<T>(string url, T data)
        {
            try
            {
                var response = await _http.PutAsJsonAsync($"{_baseUrl}/{url}", data);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<T>();
            }
            catch (Exception)
            {
                throw new Exception("Error al actualizar datos en el servidor");
            }
        }

        public async Task DeleteAsync(string url)
        {
            try
            {
                var response = await _http.DeleteAsync($"{_baseUrl}/{url}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                throw new Exception("Error al eliminar datos del servidor");
            }
        }
    }
}