namespace IncliSafe.Shared.Models.Analysis.Core
{
    public enum PredictionType
    {
        Stability,
        Safety,
        Maintenance,
        Performance,
        Anomaly,
        Trend,
        Critical,
        Warning,
        Info,
        Normal,
        High,
        Low,
        Pattern,
        Seasonal
    }

    public enum PredictionRisk
    {
        Low,
        Medium,
        High,
        Critical
    }
} 