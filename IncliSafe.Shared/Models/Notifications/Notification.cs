using System;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafe.Shared.Models.Notifications
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public NotificationSeverity Severity { get; set; }
        public NotificationType Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ReadAt { get; set; }
        public int? VehicleId { get; set; }
        public int? UserId { get; set; }
        public bool IsRead { get; set; }
        public bool IsActive { get; set; }
        public string Category { get; set; } = string.Empty;
        public NotificationPriority Priority { get; set; }
        
        public virtual Vehiculo? Vehicle { get; set; }
        public virtual Usuario? User { get; set; }

        public string SeverityString => Severity.ToString();
    }
} 