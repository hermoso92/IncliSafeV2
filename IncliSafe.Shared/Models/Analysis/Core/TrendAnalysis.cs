using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafe.Shared.Models.Analysis.Core
{
    public class TrendAnalysis
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public int DobackAnalysisId { get; set; }
        public decimal StabilityTrend { get; set; }
        public decimal SafetyTrend { get; set; }
        public decimal MaintenanceTrend { get; set; }
        public List<string> Recommendations { get; set; } = new();
        public DateTime AnalysisDate { get; set; } = DateTime.UtcNow;
        public decimal ShortTerm { get; set; }
        public decimal MediumTerm { get; set; }
        public decimal LongTerm { get; set; }
        public decimal Seasonality { get; set; }
        public List<decimal> Cycles { get; set; } = new();
        public double SeasonalityScore { get; set; }
        public double Volatility { get; set; }
        public double Confidence { get; set; }
        public string Direction { get; set; } = string.Empty;
        
        public virtual Vehiculo Vehicle { get; set; } = null!;
        public virtual DobackAnalysis Analysis { get; set; } = null!;
        public virtual ICollection<Prediction> Predictions { get; set; } = new List<Prediction>();
        public virtual ICollection<Anomaly> Anomalies { get; set; } = new List<Anomaly>();
    }
} 