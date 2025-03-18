namespace IncliSafe.Shared.Models.Enums
{
    public enum AnalysisType
    {
        Doback,
        Trend,
        Pattern,
        Anomaly
    }

    public enum CycleType
    {
        Operation,
        Maintenance,
        Test,
        Calibration
    }

    public enum TrendType
    {
        Performance,
        Stability,
        Safety,
        Maintenance
    }

    public enum PatternType
    {
        Anomaly,
        Trend,
        Cycle,
        Event
    }

    public enum AlertType
    {
        System,
        Performance,
        Safety,
        Maintenance
    }

    public enum AlertSeverity
    {
        Low,
        Medium,
        High,
        Critical
    }

    public enum PredictionType
    {
        Stability,
        Safety,
        Maintenance,
        Performance,
        System
    }

    public enum AnomalyType
    {
        Stability,
        Safety,
        Maintenance,
        Performance,
        System
    }

    public enum MaintenanceType
    {
        Preventive,
        Corrective,
        Predictive,
        Emergency
    }

    public enum MaintenanceStatus
    {
        Pending,
        InProgress,
        Completed,
        Cancelled
    }

    public enum MaintenancePriority
    {
        Low,
        Medium,
        High,
        Critical
    }

    public enum UserRole
    {
        Admin,
        Manager,
        Operator,
        Technician
    }

    public enum UserStatus
    {
        Active,
        Inactive,
        Suspended,
        Deleted
    }

    public enum NotificationType
    {
        System,
        Alert,
        Maintenance,
        Analysis
    }

    public enum AnomalySeverity
    {
        Low,
        Medium,
        High,
        Critical
    }

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

    public enum NotificationSeverity
    {
        Info,
        Warning,
        Error,
        Critical
    }

    public enum ErrorSeverity
    {
        Low,
        Medium,
        High,
        Critical
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
} 