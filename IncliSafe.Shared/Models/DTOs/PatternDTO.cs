using IncliSafe.Shared.Models.Enums;
using System;

namespace IncliSafe.Shared.Models.DTOs
{
    public class PatternDTO
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public PatternType Type { get; set; }
        public string Description { get; set; }
        public decimal Confidence { get; set; }
        public DateTime DetectedAt { get; set; }
        public string Impact { get; set; }
        public string Factors { get; set; }
        public string Recommendations { get; set; }
        public RiskLevel RiskLevel { get; set; }
        public string ActionRequired { get; set; }
        public string Documentation { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ValidatedAt { get; set; }
        public ValidationStatus ValidationStatus { get; set; }
        public string ValidatedBy { get; set; }
        public string ValidationNotes { get; set; }
    }
} 