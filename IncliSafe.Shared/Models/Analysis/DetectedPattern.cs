using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafe.Shared.Models.Analysis
{
    public class DetectedPattern
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public DateTime DetectedAt { get; set; }
        public PatternType Type { get; set; }
        public decimal Confidence { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public int? AnalysisId { get; set; }
        public virtual Vehiculo Vehicle { get; set; } = null!;
        public virtual DobackAnalysis? Analysis { get; set; }
    }
} 