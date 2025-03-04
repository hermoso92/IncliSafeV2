public class AlertSettings
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public NotificationSeverity MinimumSeverity { get; set; }
    public bool EnableEmailNotifications { get; set; }
    public bool EnablePushNotifications { get; set; }
    public decimal StabilityThreshold { get; set; }
    public decimal SafetyThreshold { get; set; }
    
    public virtual Usuario User { get; set; } = null!;
} 