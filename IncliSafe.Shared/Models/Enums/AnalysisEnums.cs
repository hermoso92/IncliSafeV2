namespace IncliSafe.Shared.Models.Enums
{
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
        Event,
        Spike,
        Drift
    }

    public enum PredictionType
    {
        Stability,
        Safety,
        Maintenance,
        Performance,
        System,
        Efficiency
    }

    public enum AnomalyType
    {
        Stability,
        Safety,
        Maintenance,
        Performance,
        System,
        Efficiency,
        Acceleration
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

    public enum PatternSeverity
    {
        Low,
        Medium,
        High,
        Critical
    }

    public enum TrendDirection
    {
        Up,
        Down,
        Stable
    }

    public enum PerformanceTrend
    {
        Improving,
        Declining,
        Stable
    }

    public enum LicenseStatus
    {
        Active,
        Expired,
        Suspended,
        Revoked,
        Pending
    }

    public enum VehicleStatus
    {
        Active,
        Inactive,
        Maintenance,
        Inspection,
        Retired
    }

    public enum NotificationType
    {
        System,
        Alert,
        Warning,
        Info,
        Success
    }

    public enum AlertPriority
    {
        Low,
        Medium,
        High,
        Critical
    }

    public enum VehicleType
    {
        Car,
        Truck,
        Bus,
        Van,
        Other
    }

    public enum VehicleCondition
    {
        New,
        Excellent,
        Good,
        Fair,
        Poor,
        Critical
    }

    public enum FuelType
    {
        Gasoline,
        Diesel,
        Electric,
        Hybrid,
        Other
    }
} 