using IncliSafe.Shared.Models.Enums;
using System;

namespace IncliSafe.Shared.Models.DTOs
{
    public class AnalysisDTO
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public DateTime AnalysisDate { get; set; }
        public AnalysisResult Result { get; set; }
        public string Description { get; set; }
        public string Recommendations { get; set; }
        public decimal Confidence { get; set; }
        public string DataSource { get; set; }
        public AnalysisType AnalysisType { get; set; }
        public AnalysisStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
} 