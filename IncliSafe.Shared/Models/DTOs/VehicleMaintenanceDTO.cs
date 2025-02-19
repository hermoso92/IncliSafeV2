using System;

namespace IncliSafe.Shared.Models.DTOs
{
    public enum MaintenanceType
    {
        Preventive,
        Corrective,
        Predictive,
        Emergency,
        Scheduled,
        Unscheduled,
        Inspection,
        Calibration,
        Upgrade,
        Other
    }

    public class VehicleMaintenanceDTO
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public MaintenanceType Type { get; set; }
        public decimal Cost { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public string? Technician { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int? WorkOrderId { get; set; }
    }
} 