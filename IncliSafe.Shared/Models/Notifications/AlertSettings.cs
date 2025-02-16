namespace IncliSafe.Shared.Models.Notifications
{
    public class AlertSettings
    {
        public double? StabilityThreshold { get; set; }
        public double? SafetyThreshold { get; set; }
        public double MaintenanceThreshold { get; set; } = 0.70;
        public bool EnableRealTimeAlerts { get; set; } = true;
        public bool EnablePredictiveAlerts { get; set; } = true;
        public bool EnableMaintenanceAlerts { get; set; } = true;
        public NotificationSeverity? MinimumSeverity { get; set; }
    }
} 