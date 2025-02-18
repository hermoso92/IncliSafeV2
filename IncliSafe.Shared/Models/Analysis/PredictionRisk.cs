namespace IncliSafe.Shared.Models.Analysis
{
    public enum PredictionRisk
    {
        Low,
        Medium,
        High,
        Critical
    }

    public static class PredictionRiskExtensions
    {
        public static string ToDisplayString(this PredictionRisk risk)
        {
            return risk switch
            {
                PredictionRisk.Low => "Bajo",
                PredictionRisk.Medium => "Medio",
                PredictionRisk.High => "Alto",
                PredictionRisk.Critical => "Crítico",
                _ => "Desconocido"
            };
        }

        public static string ToString(this PredictionRisk risk) => risk.ToDisplayString();
        public static string ToStringValue(this PredictionRisk risk) => risk.ToString();
    }
} 