using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Patterns;

namespace IncliSafe.Shared.Models.Analysis
{
    public class AnalysisResult
    {
        public int Id { get; set; }
        public decimal StabilityScore { get; set; }
        public decimal SafetyScore { get; set; }
        public decimal EfficiencyScore { get; set; }
        public decimal MaintenanceScore { get; set; }
        public List<string> Recommendations { get; set; } = new();
        public DateTime AnalysisTime { get; set; }
        public int VehicleId { get; set; }
        
        // Relación uno a uno con DobackAnalysis
        public int DobackAnalysisId { get; set; }
        public virtual DobackAnalysis DobackAnalysis { get; set; } = null!;
    }
} 