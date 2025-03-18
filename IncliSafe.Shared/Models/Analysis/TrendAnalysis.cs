using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis
{
    public class TrendAnalysis : BaseEntity
    {
        public override Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required AnalysisType Type { get; set; }
        public required Guid VehicleId { get; set; }
        public required DateTime StartTime { get; set; }
        public required DateTime EndTime { get; set; }
        public List<TrendData> DataPoints { get; set; } = new();
        public List<string> Recommendations { get; set; } = new();
        public string? Notes { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = new();
        public override DateTime CreatedAt { get; set; }
    }
} 