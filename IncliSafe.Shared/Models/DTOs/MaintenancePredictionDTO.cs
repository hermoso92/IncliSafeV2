using System;
using System.Collections.Generic;

namespace IncliSafe.Shared.Models.DTOs
{
    public class MaintenancePredictionDTO
    {
        public int VehicleId { get; set; }
        public DateTime? LastInspectionDate { get; set; }
        public DateTime PredictedMaintenanceDate { get; set; }
        public decimal MaintenanceProbability { get; set; }
        public RiskLevel RiskLevel { get; set; }
        public List<string> Recommendations { get; set; } = new();
        public int DaysUntilMaintenance => (int)(PredictedMaintenanceDate - DateTime.UtcNow).TotalDays;
        public bool IsUrgent => RiskLevel == RiskLevel.High || DaysUntilMaintenance <= 7;
    }

    public enum RiskLevel
    {
        Low,
        Medium,
        High
    }
} 