namespace IncliSafe.Shared.Models.Enums
{
    public enum WidgetType
    {
        Chart,
        Table,
        Metric,
        Alert,
        Status
    }

    public enum JobStatus
    {
        Pending,
        Running,
        Completed,
        Failed,
        Cancelled
    }

    public enum AuditAction
    {
        Create,
        Update,
        Delete,
        View,
        Execute
    }

    public enum QueueStatus
    {
        Empty,
        Processing,
        Paused,
        Error
    }

    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error,
        Critical
    }

    public enum ValidationStatus
    {
        Pending,
        Valid,
        Invalid,
        Warning
    }

    public enum ErrorSeverity
    {
        Low,
        Medium,
        High,
        Critical
    }
} 