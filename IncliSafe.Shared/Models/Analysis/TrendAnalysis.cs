using System;
using System.Collections.Generic;

namespace IncliSafe.Shared.Models.Analysis
{
    public class TrendAnalysis
    {
        public int Id { get; set; }
        public double ShortTermTrend { get; set; }
        public double MediumTermTrend { get; set; }
        public double LongTermTrend { get; set; }
        public double SeasonalityScore { get; set; }
        public double Volatility { get; set; }
        public double Confidence { get; set; }
        public DateTime AnalysisDate { get; set; }
        public int VehicleId { get; set; }
        
        // Usar virtual para lazy loading
        public virtual ICollection<Prediction> Predictions { get; set; } = new List<Prediction>();
        public virtual ICollection<Anomaly> Anomalies { get; set; } = new List<Anomaly>();
    }

    public class TrendMetric
    {
        public string Name { get; set; } = string.Empty;
        public double Value { get; set; }
        public double Change { get; set; }
        public string Direction { get; set; } = string.Empty;
    }
} 