using IncliSafe.Shared.Models.Entities;

namespace IncliSafe.Shared.Models.Auth
{
    public class AuthResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public UserSession? User { get; set; }
    }
} 