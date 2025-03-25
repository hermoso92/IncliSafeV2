using System;

namespace IncliSafe.Shared.Models.Enums
{
    public enum AnalysisResult
    {
        Unknown,
        Normal,
        Warning,
        Critical,
        MaintenanceRequired,
        InspectionRequired,
        LicenseExpired,
        LicenseExpiring,
        AnomalyDetected,
        PatternDetected,
        PredictionGenerated
    }

    public enum TrendDirection
    {
        Unknown,
        Increasing,
        Decreasing,
        Stable,
        Volatile,
        Seasonal,
        Cyclical
    }

    public enum LicenseStatus
    {
        Active,
        Expired,
        Expiring,
        Suspended,
        Revoked,
        Pending
    }

    public enum NotificationSeverity
    {
        Info,
        Warning,
        Error,
        Critical,
        Success
    }

    public enum LicenseType
    {
        Standard,
        Premium,
        Enterprise,
        Trial,
        Custom
    }

    public enum PatternType
    {
        Unknown,
        Maintenance,
        Usage,
        Performance,
        Safety,
        Compliance,
        Cost,
        Efficiency
    }

    public enum PerformanceTrend
    {
        Unknown,
        Improving,
        Declining,
        Stable,
        Volatile,
        Seasonal,
        Cyclical
    }

    public enum TrendMetric
    {
        Unknown,
        Performance,
        Efficiency,
        Safety,
        Cost,
        Usage,
        Maintenance,
        Compliance
    }

    public enum PredictionResult
    {
        Unknown,
        Stable,
        Improving,
        Declining,
        MaintenanceRequired,
        InspectionRequired,
        Critical,
        Warning
    }

    public enum NotificationPriority
    {
        Low,
        Medium,
        High,
        Critical
    }

    public enum AlertStatus
    {
        New,
        Acknowledged,
        InProgress,
        Resolved,
        Closed
    }

    public enum AlertCategory
    {
        Maintenance,
        Safety,
        Compliance,
        Performance,
        System,
        License,
        Other
    }

    public enum ValidationStatus
    {
        Pending,
        Validated,
        Failed,
        Skipped
    }

    public enum RiskLevel
    {
        Low,
        Medium,
        High,
        Critical
    }

    public enum ComplianceStatus
    {
        Compliant,
        NonCompliant,
        Pending,
        Exempt
    }

    public enum VehicleStatus
    {
        Active,
        Inactive,
        Maintenance,
        Repair,
        Sold,
        Scrapped
    }

    public enum FuelType
    {
        Gasoline,
        Diesel,
        Electric,
        Hybrid,
        CNG,
        LPG,
        Other
    }

    public enum SensorType
    {
        Temperature,
        Pressure,
        Speed,
        Acceleration,
        Brake,
        Engine,
        Fuel,
        Battery,
        GPS,
        Other
    }

    public enum SensorStatus
    {
        Active,
        Inactive,
        Faulty,
        Calibrating,
        Maintenance,
        Unknown
    }

    public enum SignalStrength
    {
        Excellent,
        Good,
        Fair,
        Poor,
        None
    }

    public enum MaintenanceType
    {
        Preventive,
        Corrective,
        Predictive,
        Emergency,
        Inspection,
        Other
    }

    public enum MaintenanceStatus
    {
        Scheduled,
        InProgress,
        Completed,
        Cancelled,
        Delayed,
        Failed
    }

    public enum InspectionStatus
    {
        Scheduled,
        InProgress,
        Passed,
        Failed,
        Cancelled,
        Pending
    }

    public enum PredictionType
    {
        Unknown,
        Maintenance,
        Performance,
        Safety,
        Cost,
        Usage,
        Compliance,
        Anomaly,
        Pattern
    }

    public enum AnalysisType
    {
        Unknown,
        Performance,
        Safety,
        Maintenance,
        Cost,
        Usage,
        Compliance,
        Pattern,
        Anomaly,
        Trend
    }

    public enum AnalysisStatus
    {
        Unknown,
        Pending,
        InProgress,
        Completed,
        Failed,
        Cancelled,
        Validated,
        Rejected
    }
} 