using IncliSafe.Shared.Models.Enums;
using System;

namespace IncliSafe.Shared.Models.DTOs
{
    public class PredictionDTO
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public string Type { get; set; }
        public decimal Value { get; set; }
        public decimal Confidence { get; set; }
        public DateTime PredictedAt { get; set; }
        public string Description { get; set; }
        public PredictionResult Result { get; set; }
        public string Factors { get; set; }
        public string Recommendations { get; set; }
        public string Impact { get; set; }
        public string RiskLevel { get; set; }
        public string ActionRequired { get; set; }
        public string Documentation { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ValidatedAt { get; set; }
        public string ValidationStatus { get; set; }
        public string ValidatedBy { get; set; }
        public string ValidationNotes { get; set; }
    }
} 