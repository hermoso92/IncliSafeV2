using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Notifications;
using IncliSafe.Shared.Models.Analysis.Core;

namespace IncliSafe.Shared.Models.Analysis
{
    public class AnalysisResult : BaseEntity
    {
        public new int Id { get; set; }
        public new DateTime CreatedAt { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string AnalysisType { get; set; }
        public required decimal StabilityScore { get; set; }
        public required decimal SafetyScore { get; set; }
        public required decimal EfficiencyScore { get; set; }
        public required decimal MaintenanceScore { get; set; }
        public required DateTime AnalysisTime { get; set; }
        public required int VehicleId { get; set; }
        public required int DobackAnalysisId { get; set; }
        public List<string> Recommendations { get; set; } = new();
        public virtual DobackAnalysis DobackAnalysis { get; set; } = null!;
    }
} 