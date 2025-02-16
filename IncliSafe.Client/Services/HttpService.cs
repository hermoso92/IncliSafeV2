using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace IncliSafe.Client.Services
{
    public class HttpService
    {
        private readonly HttpClient _http;
        private readonly string _baseUrl;

        public HttpService(HttpClient http, string baseUrl)
        {
            _http = http ?? throw new ArgumentNullException(nameof(http));
            _baseUrl = baseUrl?.TrimEnd('/') ?? throw new ArgumentNullException(nameof(baseUrl));
        }

        public async Task<T?> GetAsync<T>(string endpoint)
        {
            if (string.IsNullOrEmpty(endpoint))
                throw new ArgumentException("El endpoint no puede estar vacío", nameof(endpoint));

            return await _http.GetFromJsonAsync<T>($"{_baseUrl}/{endpoint.TrimStart('/')}");
        }

        public async Task<T?> PostAsync<T>(string endpoint, object data)
        {
            if (string.IsNullOrEmpty(endpoint))
                throw new ArgumentException("El endpoint no puede estar vacío", nameof(endpoint));
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var response = await _http.PostAsJsonAsync($"{_baseUrl}/{endpoint.TrimStart('/')}", data);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<T?> PutAsync<T>(string endpoint, T data)
        {
            if (string.IsNullOrEmpty(endpoint))
                throw new ArgumentException("El endpoint no puede estar vacío", nameof(endpoint));
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var response = await _http.PutAsJsonAsync($"{_baseUrl}/{endpoint.TrimStart('/')}", data);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task DeleteAsync(string endpoint)
        {
            if (string.IsNullOrEmpty(endpoint))
                throw new ArgumentException("El endpoint no puede estar vacío", nameof(endpoint));

            var response = await _http.DeleteAsync($"{_baseUrl}/{endpoint.TrimStart('/')}");
            response.EnsureSuccessStatusCode();
        }
    }
}