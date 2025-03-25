namespace IncliSafe.Shared.Models.Analysis.Core
{
    public enum PredictionType
    {
        Maintenance,
        Performance,
        Safety,
        Reliability,
        Efficiency,
        Custom
    }

    public enum PredictionStatus
    {
        Pending,
        InProgress,
        Completed,
        Failed,
        Cancelled
    }

    public enum PredictionAccuracy
    {
        Low,
        Medium,
        High,
        VeryHigh,
        Unknown
    }

    public enum PredictionConfidence
    {
        VeryLow,
        Low,
        Medium,
        High,
        VeryHigh
    }

    public enum PredictionTrend
    {
        Improving,
        Stable,
        Deteriorating,
        Unknown
    }

    public enum PredictionPriority
    {
        Low,
        Medium,
        High,
        Critical,
        Emergency
    }

    public enum PredictionImpact
    {
        Minimal,
        Moderate,
        Significant,
        Severe,
        Critical
    }

    public enum PredictionTimeframe
    {
        Immediate,
        ShortTerm,
        MediumTerm,
        LongTerm
    }

    public enum PredictionRisk
    {
        Low,
        Medium,
        High,
        Critical,
        Unknown
    }
} 