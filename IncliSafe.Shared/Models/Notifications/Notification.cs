using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Notifications
{
    public class Notification : BaseEntity
    {
        public required string Title { get; set; }
        public required string Message { get; set; }
        public required NotificationType Type { get; set; }
        public required NotificationStatus Status { get; set; }
        public required int UserId { get; set; }
        public DateTime? ReadAt { get; set; }
        public Dictionary<string, object> Metadata { get; set; } = new();
    }

    public enum NotificationType
    {
        Alert,
        Warning,
        Info,
        Success,
        Error
    }
} 