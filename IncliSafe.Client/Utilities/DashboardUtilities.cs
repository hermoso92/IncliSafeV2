using MudBlazor;

namespace IncliSafe.Client.Utilities
{
    public static class DashboardUtilities
    {
        public static Color GetStabilityColor(double value) =>
            value switch
            {
                > 0.8 => Color.Success,
                > 0.5 => Color.Warning,
                _ => Color.Error
            };

        public static Color GetAlertColor(int count) =>
            count switch
            {
                0 => Color.Success,
                < 3 => Color.Warning,
                _ => Color.Error
            };

        public static Color GetAnomalyColor(int count) =>
            count switch
            {
                0 => Color.Success,
                < 5 => Color.Warning,
                _ => Color.Error
            };

        public static Color GetTrendColor(decimal value) =>
            value switch
            {
                > 0 => Color.Success,
                0 => Color.Default,
                _ => Color.Error
            };

        public static string GetTrendIndicator(decimal value) =>
            value switch
            {
                > 0 => "↑",
                0 => "→",
                _ => "↓"
            };
    }
} 