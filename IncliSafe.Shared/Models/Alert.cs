namespace IncliSafe.Shared.Models
{
    public class Alert
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Message { get; set; }
        public required string Severity { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 