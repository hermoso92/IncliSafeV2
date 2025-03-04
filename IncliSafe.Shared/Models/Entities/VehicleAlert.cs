using System;

namespace IncliSafe.Shared.Models.Entities
{
    public enum AlertType
    {
        System,
        Maintenance,
        Safety,
        Vehicle,
        Inspection,
        License,
        Performance
    }

    public enum AlertSeverity
    {
        Info,
        Warning,
        Critical
    }

    public class VehicleAlert
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public AlertType Type { get; set; }
        public AlertSeverity Severity { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }
        
        public virtual Vehiculo Vehicle { get; set; } = null!;
    }
} 