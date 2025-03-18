using System;
using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace IncliSafe.Client.Services
{
    public class CacheService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly TimeSpan _defaultExpiration = TimeSpan.FromMinutes(5);

        public CacheService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task<T?> GetOrSetAsync<T>(
            string key, 
            Func<Task<T>> factory, 
            TimeSpan? expiration = null)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            var cacheKey = $"cache_{key}";
            var cached = await _localStorage.GetItemAsync<CacheEntry<T>>(cacheKey);

            if (cached != null && cached.ExpirationTime > DateTime.Now)
            {
                return cached.Value;
            }

            try
            {
                var value = await factory();
                await SetAsync(key, value, expiration ?? _defaultExpiration);
                return value;
            }
            catch (Exception)
            {
                // Si falla la obtención del valor, intentamos devolver el valor cacheado aunque esté expirado
                if (cached != null)
                {
                    return cached.Value;
                }
                throw;
            }
        }

        public async Task ClearAsync(string key)
        {
            await _localStorage.RemoveItemAsync($"cache_{key}");
        }

        public async Task ClearAllAsync()
        {
            await _localStorage.ClearAsync();
        }

        private async Task SetAsync<T>(string key, T value, TimeSpan expiration)
        {
            var entry = new CacheEntry<T>
            {
                Value = value,
                ExpirationTime = DateTime.Now.Add(expiration)
            };
            await _localStorage.SetItemAsync($"cache_{key}", entry);
        }

        private class CacheEntry<T>
        {
            public T? Value { get; set; }
            public DateTime ExpirationTime { get; set; }
        }
    }
} 