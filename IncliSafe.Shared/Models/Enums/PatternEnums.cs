namespace IncliSafe.Shared.Models.Enums
{
    public enum PatternType
    {
        Trend,
        Cycle,
        Seasonal,
        Anomaly,
        Custom
    }

    public enum PatternSeverity
    {
        Low,
        Medium,
        High,
        Critical
    }

    public enum TrendType
    {
        Performance,
        Stability,
        Safety,
        Maintenance
    }

    public enum TrendDirection
    {
        Up,
        Down,
        Stable
    }
} 