using System;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafe.Shared.Models.Notifications
{
    public enum AlertPriority
    {
        Low,
        Medium,
        High,
        Critical
    }

    public class Alert
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }
        public string? UserId { get; set; }
        public string? GroupId { get; set; }
        public string? EntityId { get; set; }
        public string? EntityType { get; set; }
        public string? ActionUrl { get; set; }
        public AlertType Type { get; set; }
        public AlertPriority Priority { get; set; }
        public AlertSeverity Severity { get; set; }
        public bool IsActive { get; set; }
        public string Category { get; set; } = string.Empty;
        
        public virtual Usuario User { get; set; } = null!;
        public virtual Vehiculo Vehicle { get; set; } = null!;
    }
} 