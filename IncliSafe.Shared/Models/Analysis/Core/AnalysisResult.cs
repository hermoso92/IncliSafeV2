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
} 