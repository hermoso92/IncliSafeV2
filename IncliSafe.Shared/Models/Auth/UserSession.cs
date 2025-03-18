using IncliSafe.Shared.Models.Enums;
using System;

namespace IncliSafe.Shared.Models.Auth
{
    public class UserSession
    {
        public required int Id { get; set; }
        public required string Name { get; set; } = string.Empty;
        public required string Username { get; set; } = string.Empty;
        public required string Email { get; set; } = string.Empty;
        public required string Role { get; set; } = string.Empty;
        public required bool IsActive { get; set; }
        public required string Token { get; set; } = string.Empty;
        public required DateTime ExpiresAt { get; set; }
        public DateTime? LastLogin { get; set; }
    }
} 

