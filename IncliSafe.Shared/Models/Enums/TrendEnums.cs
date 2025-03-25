namespace IncliSafe.Shared.Models.Analysis.Core
{
    public enum TrendType
    {
        Linear,
        Exponential,
        Seasonal,
        Cyclical,
        Random,
        Custom
    }

    public enum TrendDirection
    {
        Upward,
        Downward,
        Stable,
        Volatile,
        Unknown
    }

    public enum TrendStrength
    {
        Weak,
        Moderate,
        Strong,
        VeryStrong,
        Unknown
    }

    public enum TrendPeriod
    {
        Daily,
        Weekly,
        Monthly,
        Quarterly,
        Yearly,
        Custom
    }

    public enum TrendGranularity
    {
        Hourly,
        Daily,
        Weekly,
        Monthly,
        Quarterly,
        Yearly
    }

    public enum TrendComparison
    {
        PreviousPeriod,
        PreviousYear,
        CustomPeriod,
        Baseline
    }

    public enum TrendMetric
    {
        Count,
        Sum,
        Average,
        Maximum,
        Minimum,
        Custom
    }

    public enum TrendStatus
    {
        Active,
        Inactive,
        Pending,
        Completed,
        Failed
    }
} 