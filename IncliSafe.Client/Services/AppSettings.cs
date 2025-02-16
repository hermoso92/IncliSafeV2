namespace IncliSafe.Client.Services
{
    public class AppSettings
    {
        public string ApiBaseUrl { get; set; } = string.Empty;
        public CacheSettings CacheSettings { get; set; } = new();
    }

    public class CacheSettings
    {
        public int DefaultExpirationMinutes { get; set; } = 5;
    }
} 