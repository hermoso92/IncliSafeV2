
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Text.Json;
using IncliSafe.Cliente.Models;

namespace IncliSafe.Cliente.Services
{
    public class HttpService
    {
        private readonly HttpClient _http;
        private readonly string _baseUrl = "http://0.0.0.0:80/api";

        public HttpService(HttpClient http)
        {
            _http = http;
        }

        public async Task<T> GetAsync<T>(string url)
        {
            var response = await _http.GetFromJsonAsync<T>($"{_baseUrl}/{url}");
            return response;
        }

        public async Task<T> PostAsync<T>(string url, T data)
        {
            var response = await _http.PostAsJsonAsync($"{_baseUrl}/{url}", data);
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<T> PutAsync<T>(string url, T data)
        {
            var response = await _http.PutAsJsonAsync($"{_baseUrl}/{url}", data);
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task DeleteAsync(string url)
        {
            await _http.DeleteAsync($"{_baseUrl}/{url}");
        }
    }
}
