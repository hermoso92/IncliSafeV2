using IncliSafe.Shared.Models.Enums;
using System;
using IncliSafe.Shared.Models.DTOs;
using IncliSafe.Shared.Models.Common;

namespace IncliSafe.Shared.Models.Entities
{
    public class Prediction : BaseEntity
    {
        public required int VehicleId { get; set; }
        public required PredictionType PredictionType { get; set; }
        public required DateTime PredictedDate { get; set; }
        public required decimal Probability { get; set; }
        public required RiskLevel RiskLevel { get; set; }
        public required string Recommendations { get; set; } = string.Empty;
        
        public virtual Vehiculo Vehicle { get; set; } = null!;
    }
} 

