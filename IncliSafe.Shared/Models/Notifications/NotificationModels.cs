using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Notifications
{
    public class Alert : BaseEntity
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required IncliSafe.Shared.Models.Enums.AlertSeverity Severity { get; set; }
        public required string Source { get; set; }
        public required string Category { get; set; }
        public required bool IsActive { get; set; }
        public required DateTime AlertDate { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public string? Resolution { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = new();
        public List<decimal> DataPoints { get; set; } = new();
        public List<DateTime> Timestamps { get; set; } = new();
        public Dictionary<string, object> Metadata { get; set; } = new();
    }

    public class Notification : BaseEntity
    {
        public required string Title { get; set; }
        public required string Message { get; set; }
        public required IncliSafe.Shared.Models.Enums.NotificationType Type { get; set; }
        public required IncliSafe.Shared.Models.Enums.AlertPriority Priority { get; set; }
        public required string Recipient { get; set; }
        public required bool IsRead { get; set; }
        public required DateTime SentDate { get; set; }
        public DateTime? ReadDate { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = new();
        public List<decimal> DataPoints { get; set; } = new();
        public List<DateTime> Timestamps { get; set; } = new();
        public Dictionary<string, object> Metadata { get; set; } = new();
    }

    public class NotificationSettings : BaseEntity
    {
        public required string UserId { get; set; }
        public required bool EmailEnabled { get; set; }
        public required bool SMSEnabled { get; set; }
        public required bool PushEnabled { get; set; }
        public required string EmailAddress { get; set; }
        public required string PhoneNumber { get; set; }
        public required string PushToken { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = new();
        public List<decimal> DataPoints { get; set; } = new();
        public List<DateTime> Timestamps { get; set; } = new();
        public Dictionary<string, object> Metadata { get; set; } = new();
    }
} 