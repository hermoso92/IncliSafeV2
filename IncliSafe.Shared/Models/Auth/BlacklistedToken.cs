using IncliSafe.Shared.Models.Enums;
namespace IncliSafe.Shared.Models.Auth
{
    public class BlacklistedToken
    {
        public required int Id { get; set; }
        public required string Token { get; set; }
        public required DateTime RevokedAt { get; set; }
        public required DateTime ExpiresAt { get; set; }
    }
} 

