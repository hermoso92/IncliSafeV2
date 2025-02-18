using System;

namespace IncliSafe.Shared.Models
{
    public class Alert
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public string UserId { get; set; } = string.Empty;
        public virtual Usuario? User { get; set; }
    }
} 