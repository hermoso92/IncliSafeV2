
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using IncliSafe.Cliente.Models;

namespace IncliSafe.Cliente.Services
{
    public class HttpService
    {
        private readonly HttpClient _http;

        public HttpService(HttpClient http)
        {
            _http = http;
        }

        public async Task<T[]> GetAsync<T>(string url)
        {
            return await _http.GetFromJsonAsync<T[]>(url);
        }

        public async Task<T> PostAsync<T>(string url, T data)
        {
            var response = await _http.PostAsJsonAsync(url, data);
            return await response.Content.ReadFromJsonAsync<T>();
        }
    }
}
