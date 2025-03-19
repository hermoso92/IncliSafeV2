using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Entities
{
    public class Mantenimiento : BaseEntity
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required MaintenanceType Type { get; set; }
        public required MaintenanceStatus Status { get; set; }
        public required MaintenancePriority Priority { get; set; }
        public required int VehicleId { get; set; }
        public required DateTime ScheduledDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string? Notes { get; set; }
        public required decimal EstimatedCost { get; set; }
        public decimal? ActualCost { get; set; }
        public required int EstimatedDuration { get; set; }
        public int? ActualDuration { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = new();
        public virtual Vehicle Vehicle { get; set; } = null!;
        public virtual ICollection<Inspeccion> Inspecciones { get; set; } = new List<Inspeccion>();
    }
} 