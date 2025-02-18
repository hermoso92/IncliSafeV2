using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Patterns;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafe.Shared.Models.Analysis
{
    public class AnalysisResult
    {
        public int Id { get; set; }
        public int DobackAnalysisId { get; set; }
        public int VehicleId { get; set; }
        public decimal StabilityScore { get; set; }
        public decimal SafetyScore { get; set; }
        public decimal EfficiencyScore { get; set; }
        public decimal MaintenanceScore { get; set; }
        public DateTime AnalysisTime { get; set; }
        public List<string> Recommendations { get; set; } = new();
        
        public virtual DobackAnalysis DobackAnalysis { get; set; } = null!;
        public virtual Vehiculo Vehicle { get; set; } = null!;

        public decimal GetStabilityScore() => StabilityScore;
        public decimal GetSafetyScore() => SafetyScore;
        public decimal GetMaintenanceScore() => MaintenanceScore;
    }
} 