namespace IncliSafe.Shared.Models.Notifications
{
    public class NotificationSettings
    {
        public int Id { get; set; }
        public int VehiculoId { get; set; }
        public bool EnableNotifications { get; set; } = true;
        public double StabilityThreshold { get; set; } = 0.7;
        public double SafetyThreshold { get; set; } = 0.8;
        public NotificationSeverity? MinimumSeverity { get; set; } = NotificationSeverity.Warning;
        public virtual Vehiculo? Vehiculo { get; set; }
    }
} 