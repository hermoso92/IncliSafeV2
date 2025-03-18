using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;

namespace IncliSafe.Shared.Models.Entities
{
    public class SensorReading : BaseEntity
    {
        public required int VehicleId { get; set; }
        public required DateTime Timestamp { get; set; }
        public required string SensorType { get; set; }
        public required decimal Value { get; set; }
        public required string Unit { get; set; }
        public decimal? Threshold { get; set; }
        public AlertSeverity? Severity { get; set; }
        public virtual Vehicle Vehicle { get; set; } = null!;
        public virtual ICollection<Alert> Alerts { get; set; } = new List<Alert>();
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
} 