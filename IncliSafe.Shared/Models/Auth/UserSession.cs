using System;

namespace IncliSafe.Shared.Models.Auth
{
    public class UserSession
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime LastLogin { get; set; } = DateTime.UtcNow;
    }
} 