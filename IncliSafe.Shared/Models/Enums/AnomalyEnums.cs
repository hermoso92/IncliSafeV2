namespace IncliSafe.Shared.Models.Analysis.Core
{
    public enum AnomalyType
    {
        Point,
        Contextual,
        Collective,
        Seasonal,
        Trend,
        Custom
    }

    public enum AnomalySeverity
    {
        Low,
        Medium,
        High,
        Critical,
        Emergency
    }

    public enum AnomalyStatus
    {
        Detected,
        Investigating,
        Confirmed,
        Resolved,
        FalsePositive,
        Ignored
    }

    public enum AnomalyCategory
    {
        Performance,
        Safety,
        Maintenance,
        Operational,
        Environmental,
        Custom
    }

    public enum AnomalyDetectionMethod
    {
        Statistical,
        MachineLearning,
        RuleBased,
        Hybrid,
        Custom
    }

    public enum AnomalyConfidence
    {
        VeryLow,
        Low,
        Medium,
        High,
        VeryHigh
    }

    public enum AnomalyImpact
    {
        Minimal,
        Moderate,
        Significant,
        Severe,
        Critical
    }

    public enum AnomalyResolution
    {
        Automatic,
        Manual,
        Hybrid,
        Pending,
        NotApplicable
    }
} 