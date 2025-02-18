using System;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafe.Shared.Models.Notifications
{
    public class Alert
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public NotificationSeverity Severity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ReadAt { get; set; }
        public bool IsActive { get; set; }
        public int UserId { get; set; }
        public int VehicleId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Category { get; set; } = string.Empty;
        public virtual Usuario User { get; set; } = null!;
        public virtual Vehiculo Vehicle { get; set; } = null!;
    }
} 