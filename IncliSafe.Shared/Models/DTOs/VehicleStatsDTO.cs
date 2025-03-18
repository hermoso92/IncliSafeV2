using IncliSafe.Shared.Models.Enums;
using System;
using System.Collections.Generic;

namespace IncliSafe.Shared.Models.DTOs
{
    public class VehicleStatsDTO
    {
        public required int VehicleId { get; set; }
        public required int TotalInspections { get; set; }
        public DateTime? LastInspectionDate { get; set; }
        public required decimal AverageStabilityScore { get; set; }
        public required decimal AverageSafetyScore { get; set; }
        public required int TotalAnalyses { get; set; }
        public DateTime? LastAnalysisDate { get; set; }
        public List<MonthlyStatsDTO> MonthlyStats { get; set; } = new();
        public required LicenseValidationDTO LicenseStatus { get; set; } = null!;
    }

    public class MonthlyStatsDTO
    {
        public required DateTime Month { get; set; }
        public required int InspectionsCount { get; set; }
        public required int AnalysesCount { get; set; }
        public required decimal AverageStabilityScore { get; set; }
        public required decimal AverageSafetyScore { get; set; }
        public List<string> Incidents { get; set; } = new();
    }
} 

