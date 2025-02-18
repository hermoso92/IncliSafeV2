using System;
using System.Collections.Generic;

namespace IncliSafe.Shared.Models.DTOs
{
    public class VehicleStatsDTO
    {
        public int VehicleId { get; set; }
        public int TotalInspections { get; set; }
        public DateTime? LastInspectionDate { get; set; }
        public decimal AverageStabilityScore { get; set; }
        public decimal AverageSafetyScore { get; set; }
        public int TotalAnalyses { get; set; }
        public DateTime? LastAnalysisDate { get; set; }
        public List<MonthlyStatsDTO> MonthlyStats { get; set; } = new();
        public LicenseValidationDTO LicenseStatus { get; set; } = null!;
    }

    public class MonthlyStatsDTO
    {
        public DateTime Month { get; set; }
        public int InspectionsCount { get; set; }
        public int AnalysesCount { get; set; }
        public decimal AverageStabilityScore { get; set; }
        public decimal AverageSafetyScore { get; set; }
        public List<string> Incidents { get; set; } = new();
    }
} 