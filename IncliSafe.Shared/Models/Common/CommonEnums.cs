using IncliSafe.Shared.Models.Enums;
namespace IncliSafe.Shared.Models.Common
{
    public enum AnalysisType
    {
        Doback,
        Trend,
        Pattern,
        Predictive
    }

    public enum AnomalyType
    {
        Stability,
        Safety,
        Maintenance,
        Performance,
        System
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
        Maintenance,
        Safety,
        Performance
    }

    public enum TrendDirection
    {
        Improving,
        Stable,
        Declining
    }

    public enum PerformanceTrend
    {
        Excellent,
        Good,
        Fair,
        Poor
    }

    public enum PatternType
    {
        Seasonal,
        Cyclical,
        Trend,
        Anomaly
    }
} 


