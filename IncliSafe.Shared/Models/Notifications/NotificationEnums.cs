namespace IncliSafe.Shared.Models.Notifications
{
    public enum NotificationSeverity
    {
        Info,
        Success,
        Warning,
        Error,
        Critical
    }

    public enum NotificationType
    {
        System,
        Alert,
        Warning,
        Info,
        PatternDetected,
        Anomaly,
        Maintenance,
        Vehicle,
        Safety
    }

    public enum NotificationPriority
    {
        Low,
        Medium,
        High
    }
} 