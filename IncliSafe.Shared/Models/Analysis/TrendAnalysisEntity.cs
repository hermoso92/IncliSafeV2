using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Analysis.Core;

namespace IncliSafe.Shared.Models.Analysis
{
    public class TrendAnalysisEntity
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public int DobackAnalysisId { get; set; }
        public decimal StabilityTrend { get; set; }
        public decimal SafetyTrend { get; set; }
        public decimal MaintenanceTrend { get; set; }
        public List<string> Recommendations { get; set; } = new();
        public DateTime AnalysisDate { get; set; }
        public virtual DobackAnalysis Analysis { get; set; } = null!;
    }
} 