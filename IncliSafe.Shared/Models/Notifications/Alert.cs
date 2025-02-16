using System;

namespace IncliSafe.Shared.Models.Notifications
{
    public class Alert
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public NotificationSeverity Severity { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public int VehicleId { get; set; }
        public int? UserId { get; set; }
    }
} 