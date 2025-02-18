using System;

namespace IncliSafe.Shared.Models.Auth
{
    public class UserSession
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool Activo { get; set; }
    }
} 