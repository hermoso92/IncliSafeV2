namespace IncliSafe.Shared.Models.Enums
{
    public enum NotificationSeverity
    {
        Info,
        Warning,
        Error,
        Critical
    }

    public enum AlertSeverity
    {
        Info,
        Warning,
        Error,
        Critical
    }

    public enum NotificationFrequency
    {
        Immediately,
        Daily,
        Weekly,
        Monthly
    }

    public enum NotificationType
    {
        System,
        Maintenance,
        Inspection,
        Alert,
        Custom
    }

    public enum NotificationStatus
    {
        Pending,
        Sent,
        Failed,
        Read,
        Archived
    }

    public enum NotificationChannel
    {
        Email,
        SMS,
        Push,
        InApp,
        Webhook
    }

    public enum AlertPriority
    {
        Low,
        Medium,
        High,
        Critical
    }
} 