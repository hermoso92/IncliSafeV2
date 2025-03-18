using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;

namespace IncliSafe.Shared.Models.Entities
{
    public class VehicleMetrics : BaseEntity
    {
        public required int VehicleId { get; set; }
        public required DateTime Timestamp { get; set; }
        public required decimal StabilityScore { get; set; }
        public required decimal SafetyScore { get; set; }
        public required decimal PerformanceScore { get; set; }
        public required decimal MaintenanceScore { get; set; }
        public required decimal OverallScore { get; set; }
        public virtual Vehicle Vehicle { get; set; } = null!;
    }
} 

