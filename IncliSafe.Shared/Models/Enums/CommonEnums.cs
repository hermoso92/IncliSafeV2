using IncliSafe.Shared.Models.Enums;
namespace IncliSafe.Shared.Models.Enums
{
    public enum PredictionType
    {
        Stability,
        Safety,
        Maintenance,
        Performance,
        Efficiency
    }

    public enum AlertType
    {
        System,
        Vehicle,
        Maintenance,
        Safety,
        Performance
    }

    public enum AnalysisType
    {
        Doback,
        Trend,
        Pattern,
        Prediction
    }

    public enum PatternType
    {
        Anomaly,
        Trend,
        Cycle,
        Spike,
        Drift
    }

    public enum AnomalyType
    {
        Stability,
        Safety,
        Maintenance,
        Performance,
        Efficiency,
        Acceleration
    }

    public enum AlertSeverity
    {
        Low,
        Medium,
        High,
        Critical
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

    public enum NotificationSeverity
    {
        Low,
        Medium,
        High,
        Critical
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

    public enum MaintenanceType
    {
        Preventive,
        Corrective,
        Scheduled,
        Emergency,
        Inspection,
        Upgrade
    }
} 
