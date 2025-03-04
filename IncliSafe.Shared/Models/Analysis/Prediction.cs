using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafe.Shared.Models.Analysis
{
    public class Prediction
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal PredictedValue { get; set; }
        public decimal Confidence { get; set; }
        public string MetricType { get; set; } = string.Empty;
        public virtual Vehiculo Vehicle { get; set; } = null!;
    }

    public enum PredictionType
    {
        Stability,
        Safety,
        Maintenance,
        Performance,
        Efficiency
    }

    public enum PredictionRisk
    {
        Low,
        Medium,
        High,
        Critical
    }
} 