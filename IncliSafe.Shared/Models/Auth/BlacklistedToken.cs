namespace IncliSafe.Shared.Models.Auth
{
    public class BlacklistedToken
    {
        public int Id { get; set; }
        public required string Token { get; set; }
        public DateTime RevokedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
} 