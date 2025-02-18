namespace IncliSafe.Shared.Models.Notifications
{
    public class NotificationSettings
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public bool Enabled { get; set; }
        
        // Thresholds
        public decimal WarningThreshold { get; set; }
        public decimal CriticalThreshold { get; set; }
        public decimal StabilityThreshold { get; set; } = 0.7m;
        public decimal SafetyThreshold { get; set; } = 0.8m;
        
        // Notification Configuration
        public NotificationSeverity MinimumSeverity { get; set; } = NotificationSeverity.Warning;
        public NotificationPriority Priority { get; set; } = NotificationPriority.Medium;
        public int MinutesBetweenNotifications { get; set; } = 15;
        
        // Channel Settings
        public bool EnableEmail { get; set; }
        public bool EnablePush { get; set; }
        public bool EnableSMS { get; set; }
        
        public virtual Vehiculo Vehicle { get; set; } = null!;
    }
} 