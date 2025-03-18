using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Notifications;
using IncliSafe.Shared.Models.Analysis.Core;

namespace IncliSafe.Shared.Models.Analysis
{
    public class Anomaly : BaseEntity
    {
        public new int Id { get; set; }
        public new DateTime CreatedAt { get; set; }
        public required int VehicleId { get; set; }
        public required DateTime DetectedAt { get; set; }
        public required AnomalyType Type { get; set; }
        public required AlertSeverity Severity { get; set; }
        public required string Description { get; set; } = string.Empty;
        public required decimal ExpectedValue { get; set; }
        public required decimal ActualValue { get; set; }
        public required decimal Deviation { get; set; }
        public int? AnalysisId { get; set; }
        public virtual Vehicle Vehicle { get; set; } = null!;
        public virtual DobackAnalysis? Analysis { get; set; }
    }
} 