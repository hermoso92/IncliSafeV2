using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Notifications
{
    public class NotificationSettings : BaseEntity
    {
        public required int UserId { get; set; }
        public required bool EmailEnabled { get; set; }
        public required bool PushEnabled { get; set; }
        public required bool SMSEnabled { get; set; }
        public required AlertSeverity MinimumSeverity { get; set; }
        public List<string> NotificationTypes { get; set; } = new();
        public Dictionary<string, bool> NotificationPreferences { get; set; } = new();
    }
} 