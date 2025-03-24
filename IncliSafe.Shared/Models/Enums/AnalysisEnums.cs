namespace IncliSafe.Shared.Models.Enums
{
    public enum AnalysisType
    {
        Trend,
        Pattern,
        Anomaly,
        Prediction,
        Performance,
        Maintenance,
        Safety,
        Compliance
    }

    public enum AnalysisCategory
    {
        Technical,
        Operational,
        Safety,
        Compliance,
        Performance,
        Maintenance,
        Financial,
        Environmental
    }

    public enum AnalysisCalculation
    {
        Average,
        Sum,
        Count,
        Min,
        Max,
        StandardDeviation,
        Variance,
        Median,
        Mode,
        Custom
    }

    public enum AnalysisDimension
    {
        Time,
        Space,
        Category,
        Metric,
        Source,
        Target,
        Group,
        Custom
    }

    public enum AnalysisFilter
    {
        DateRange,
        Category,
        Source,
        Target,
        Status,
        Priority,
        Custom
    }

    public enum AnalysisFormat
    {
        Table,
        Chart,
        Report,
        Export,
        Custom
    }

    public enum AnalysisGroup
    {
        ByDate,
        ByCategory,
        BySource,
        ByTarget,
        ByStatus,
        Custom
    }

    public enum AnalysisImport
    {
        CSV,
        Excel,
        JSON,
        XML,
        Custom
    }

    public enum AnalysisLevel
    {
        System,
        Component,
        Subsystem,
        Detail,
        Custom
    }

    public enum AnalysisMetric
    {
        Count,
        Percentage,
        Rate,
        Ratio,
        Custom
    }

    public enum AnalysisMode
    {
        RealTime,
        Batch,
        Scheduled,
        OnDemand,
        Custom
    }

    public enum AnalysisPeriod
    {
        Daily,
        Weekly,
        Monthly,
        Quarterly,
        Yearly,
        Custom
    }

    public enum AnalysisScope
    {
        Global,
        Regional,
        Local,
        Custom
    }

    public enum AnalysisSort
    {
        Ascending,
        Descending,
        Custom
    }

    public enum AnalysisSource
    {
        Sensor,
        Manual,
        External,
        System,
        Custom
    }

    public enum AnalysisResult
    {
        Success,
        Warning,
        Error,
        Custom
    }

    public enum AnalysisTarget
    {
        Vehicle,
        Component,
        System,
        Process,
        Custom
    }

    public enum AnalysisPriority
    {
        Low,
        Medium,
        High,
        Critical,
        Custom
    }

    public enum AnalysisTransformation
    {
        Normalize,
        Scale,
        Filter,
        Aggregate,
        Custom
    }

    public enum AnalysisUnit
    {
        Count,
        Percentage,
        Time,
        Distance,
        Speed,
        Custom
    }

    public enum AnalysisValidation
    {
        Required,
        Range,
        Format,
        Custom
    }

    public enum AnalysisThreshold
    {
        Warning,
        Critical,
        Custom
    }

    public enum AnalysisFrequency
    {
        RealTime,
        Hourly,
        Daily,
        Weekly,
        Monthly,
        Custom
    }

    public enum AnalysisExport
    {
        CSV,
        Excel,
        PDF,
        JSON,
        XML,
        Custom
    }

    public enum AnalysisChart
    {
        Line,
        Bar,
        Pie,
        Scatter,
        Custom
    }

    public enum AnalysisSeverity
    {
        Low,
        Medium,
        High,
        Critical
    }
} 