public class PredictionResult
{
    public List<double> Predictions { get; set; } = new();
    public List<double> UpperBound { get; set; } = new();
    public List<double> LowerBound { get; set; } = new();
    public decimal Confidence { get; set; }
    public double Trend { get; set; }
    public List<DateTime> TimePoints { get; set; } = new();
    public List<string> Recommendations { get; set; } = new();
}

public class Anomaly
{
    public DateTime Timestamp { get; set; }
    public double Value { get; set; }
    public double ExpectedValue { get; set; }
    public double Deviation { get; set; }
    public AnomalyType Type { get; set; }
    public string Description { get; set; } = "";
    public List<string> PossibleCauses { get; set; } = new();
    public List<string> RecommendedActions { get; set; } = new();
}

public enum AnomalyType
{
    High,
    Low,
    Pattern,
    Seasonal,
    Trend
}

public class TrendAnalysis
{
    public Trend ShortTerm { get; set; } = new();
    public Trend MediumTerm { get; set; } = new();
    public Trend LongTerm { get; set; } = new();
    public Seasonality Seasonality { get; set; } = new();
    public List<Cycle> Cycles { get; set; } = new();
    public List<string> Insights { get; set; } = new();
    public RiskAssessment Risk { get; set; } = new();
}

public class Trend
{
    public double Slope { get; set; }
    public double Intercept { get; set; }
    public double R2 { get; set; }
    public TrendDirection Direction => Slope switch
    {
        > 0.01 => TrendDirection.Increasing,
        < -0.01 => TrendDirection.Decreasing,
        _ => TrendDirection.Stable
    };
    public TrendStrength Strength => Math.Abs(Slope) switch
    {
        > 0.1 => TrendStrength.Strong,
        > 0.05 => TrendStrength.Moderate,
        _ => TrendStrength.Weak
    };
}

public enum TrendDirection
{
    Increasing,
    Decreasing,
    Stable
}

public enum TrendStrength
{
    Strong,
    Moderate,
    Weak
}

public class Seasonality
{
    public bool Detected { get; set; }
    public int Period { get; set; }
    public double Strength { get; set; }
    public List<SeasonalPattern> Patterns { get; set; } = new();
}

public class SeasonalPattern
{
    public string Name { get; set; } = "";
    public int StartPeriod { get; set; }
    public int EndPeriod { get; set; }
    public double Magnitude { get; set; }
    public string Description { get; set; } = "";
}

public class Cycle
{
    public string Name { get; set; } = "";
    public double Period { get; set; }
    public double Amplitude { get; set; }
    public double Phase { get; set; }
    public CycleType Type { get; set; }
    public string Description { get; set; } = "";
}

public enum CycleType
{
    Daily,
    Weekly,
    Monthly,
    Quarterly,
    Annual,
    Custom
}

public class Pattern
{
    public string Name { get; set; } = "";
    public PatternType Type { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public double Confidence { get; set; }
    public List<double> Values { get; set; } = new();
    public string Description { get; set; } = "";
    public List<string> Implications { get; set; } = new();
}

public enum PatternType
{
    Repetitive,
    Trend,
    Cyclic,
    Seasonal,
    Anomalous
}

public class RiskAssessment
{
    public RiskLevel Level { get; set; }
    public List<RiskFactor> Factors { get; set; } = new();
    public string Summary { get; set; } = "";
    public List<string> Recommendations { get; set; } = new();
    public double Score { get; set; }
}

public enum RiskLevel
{
    Low,
    Medium,
    High,
    Critical
}

public class RiskFactor
{
    public string Name { get; set; } = "";
    public double Impact { get; set; }
    public double Probability { get; set; }
    public string Description { get; set; } = "";
    public List<string> MitigationStrategies { get; set; } = new();
} 