using System;

namespace IncliSafe.Shared.Models.Entities
{
    public class VehicleMetrics
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public int TotalAnalyses { get; set; }
        public decimal AverageStabilityScore { get; set; }
        public decimal AverageSafetyScore { get; set; }
        public DateTime? LastAnalysisDate { get; set; }
        
        public virtual Vehiculo Vehicle { get; set; } = null!;
    }
} 