using System;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis
{
    public class AnalysisPattern : BaseEntity
    {
        public override Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required PatternType Type { get; set; }
        public required AlertSeverity Severity { get; set; }
        public required Guid VehicleId { get; set; }
        public required Guid AnalysisId { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = new();
        public override DateTime CreatedAt { get; set; }
    }
} 