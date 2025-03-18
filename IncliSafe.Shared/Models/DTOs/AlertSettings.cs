using IncliSafe.Shared.Models.Enums;
using System;
using System.Collections.Generic;

namespace IncliSafe.Shared.Models.DTOs
{
    public class AlertSettings
    {
        public required int Id { get; set; }
        public required int VehicleId { get; set; }
        public required bool EnableMaintenanceAlerts { get; set; }
        public required bool EnableInspectionAlerts { get; set; }
        public required bool EnablePerformanceAlerts { get; set; }
        public required int MaintenanceAlertDays { get; set; }
        public required int InspectionAlertDays { get; set; }
        public required decimal PerformanceThreshold { get; set; }
        public List<string> NotificationChannels { get; set; } = new();
        public Dictionary<string, string> CustomSettings { get; set; } = new();
    }

    public class AlertThreshold
    {
        public required int Id { get; set; }
        public required string MetricName { get; set; }
        public required decimal MinValue { get; set; }
        public required decimal MaxValue { get; set; }
        public required int SeverityLevel { get; set; }
        public required string AlertMessage { get; set; }
    }

    public class AlertRule
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Condition { get; set; }
        public required string Action { get; set; }
        public required bool IsEnabled { get; set; }
        public required int Priority { get; set; }
    }
} 

