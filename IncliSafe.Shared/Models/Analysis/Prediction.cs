using IncliSafe.Shared.Models.Enums;
using System;
using IncliSafe.Shared.Models.Common;

namespace IncliSafe.Shared.Models.Analysis
{
    public class Prediction
    {
        public required int Id { get; set; }
        public required int VehicleId { get; set; }
        public required DateTime Timestamp { get; set; }
        public required decimal PredictedValue { get; set; }
        public required decimal Confidence { get; set; }
        public required string MetricType { get; set; }
    }

    public enum PredictionRisk
    {
        Low,
        Medium,
        High,
        Critical
    }
} 


