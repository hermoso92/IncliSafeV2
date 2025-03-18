using IncliSafe.Shared.Models.Enums;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafe.Shared.Models.Auth
{
    public class AuthResponse
    {
        public required bool Success { get; set; }
        public required string Message { get; set; } = string.Empty;
        public required string Token { get; set; } = string.Empty;
        public UserSession? User { get; set; }
    }
} 

