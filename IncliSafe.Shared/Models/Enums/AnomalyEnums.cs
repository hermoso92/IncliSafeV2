namespace IncliSafe.Shared.Models.Enums
{
    public enum AnomalyType
    {
        Mechanical,
        Electrical,
        Structural,
        Software,
        Sensor,
        Unknown
    }

    public enum AnomalySeverity
    {
        Low,
        Medium,
        High,
        Critical
    }

    public enum PredictionType
    {
        Maintenance,
        Performance,
        Safety,
        Reliability
    }

    public enum PredictionRisk
    {
        Low,
        Medium,
        High,
        Critical
    }
} 