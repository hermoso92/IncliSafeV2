using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Entities
{
    public class Mantenimiento : BaseEntity
    {
        public override Guid Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public required MaintenanceType Type { get; set; }
        public required MaintenanceStatus Status { get; set; }
        public required MaintenancePriority Priority { get; set; }
        public required DateTime ScheduledDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public required Guid VehicleId { get; set; }
        public required Guid AssignedToId { get; set; }
        public List<string> Tasks { get; set; } = new();
        public List<string> Notes { get; set; } = new();
        public Dictionary<string, object> Parameters { get; set; } = new();
        public virtual Vehicle Vehicle { get; set; } = null!;
        public virtual ICollection<Inspeccion> Inspecciones { get; set; } = new List<Inspeccion>();
    }
} 