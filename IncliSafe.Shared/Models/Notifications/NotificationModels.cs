using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Notifications
{
    public class Notification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public required string Title { get; set; }
        public required string Message { get; set; }
        public NotificationType Type { get; set; }
        public NotificationStatus Status { get; set; }
        public NotificationChannel Channel { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ReadAt { get; set; }
    }

    public class Alert
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public AlertSeverity Severity { get; set; }
        public AlertPriority Priority { get; set; }
        public DateTime CreatedAt { get; set; }
        public required string Resolution { get; set; }
        public bool IsResolved { get; set; }
        public DateTime? ResolvedAt { get; set; }
    }

    public class NotificationSettings
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool EmailNotifications { get; set; }
        public bool PushNotifications { get; set; }
        public NotificationFrequency Frequency { get; set; }
        public List<NotificationType> EnabledTypes { get; set; } = new();
        public DateTime LastNotificationSent { get; set; }
    }
} 