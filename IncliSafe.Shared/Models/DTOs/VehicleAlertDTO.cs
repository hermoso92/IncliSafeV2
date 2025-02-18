using System;

namespace IncliSafe.Shared.Models.DTOs
{
    public class VehicleAlertDTO
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
    }

    public enum AlertType
    {
        Inspection,
        Maintenance,
        License,
        Safety,
        Performance
    }

    public enum AlertSeverity
    {
        Info,
        Warning,
        Critical
    }
} 