using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Notifications;
using IncliSafe.Shared.Models.Analysis.Core;

namespace IncliSafe.Shared.Models.Analysis
{
    public class AnalysisBase : BaseEntity
    {
        public new int Id { get; set; }
        public new DateTime CreatedAt { get; set; }
        public required int VehicleId { get; set; }
        public required DateTime AnalysisDate { get; set; }
        public required AnalysisType Type { get; set; }
        public required decimal StabilityScore { get; set; }
        public required decimal SafetyScore { get; set; }
        public required decimal MaintenanceScore { get; set; }
        public string? Notes { get; set; }
        public virtual Vehicle Vehicle { get; set; } = null!;
    }
} 