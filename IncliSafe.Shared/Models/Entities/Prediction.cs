using System;
using IncliSafe.Shared.Models.DTOs;

namespace IncliSafe.Shared.Models.Entities
{
    public class Prediction
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public PredictionType PredictionType { get; set; }
        public DateTime PredictedDate { get; set; }
        public decimal Probability { get; set; }
        public RiskLevel RiskLevel { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Recommendations { get; set; } = string.Empty;
        
        public virtual Vehiculo Vehicle { get; set; } = null!;
    }

    public enum PredictionType
    {
        Maintenance,
        Safety,
        Performance
    }
} 