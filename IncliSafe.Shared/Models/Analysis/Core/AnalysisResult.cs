using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Patterns;

namespace IncliSafe.Shared.Models.Analysis.Core
{
    public class AnalysisResult
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public decimal StabilityScore { get; set; }
        public decimal SafetyScore { get; set; }
        public DateTime AnalysisTime { get; set; }
        public List<string> Recommendations { get; set; } = new();
        public List<DobackData> Data { get; set; } = new();
        public List<DetectedPattern> Patterns { get; set; } = new();
        public List<Anomaly> Anomalies { get; set; } = new();
    }
} 