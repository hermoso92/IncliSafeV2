using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Analysis.Core;

namespace IncliSafe.Shared.Models.Notifications
{
    public class Alert : BaseEntity
    {
        public new int Id { get; set; }
        public new DateTime CreatedAt { get; set; }
        public required string Title { get; set; }
        public required string Message { get; set; }
        public required AlertSeverity Severity { get; set; }
        public required DateTime Timestamp { get; set; }
        public required bool IsAcknowledged { get; set; }
        public required string AcknowledgedBy { get; set; }
        public DateTime? AcknowledgedAt { get; set; }
        public List<SensorReading> RelatedReadings { get; set; } = new();
    }

    public class Notification : BaseEntity
    {
        public new int Id { get; set; }
        public new DateTime CreatedAt { get; set; }
        public required string Title { get; set; }
        public required string Message { get; set; }
        public required AlertSeverity Severity { get; set; }
        public required DateTime Timestamp { get; set; }
        public required bool IsRead { get; set; }
        public required string ReadBy { get; set; }
        public DateTime? ReadAt { get; set; }
        public List<SensorReading> RelatedReadings { get; set; } = new();
    }

    public class AlertConfiguration : BaseEntity
    {
        public new int Id { get; set; }
        public new DateTime CreatedAt { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required AlertSeverity Severity { get; set; }
        public required bool IsEnabled { get; set; }
        public required string Condition { get; set; }
        public required string Action { get; set; }
        public Dictionary<string, string> Parameters { get; set; } = new();
    }

    public class NotificationConfiguration : BaseEntity
    {
        public new int Id { get; set; }
        public new DateTime CreatedAt { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required AlertSeverity Severity { get; set; }
        public required bool IsEnabled { get; set; }
        public required string Condition { get; set; }
        public required string Action { get; set; }
        public Dictionary<string, string> Parameters { get; set; } = new();
    }

    public class AlertTemplate : BaseEntity
    {
        public new int Id { get; set; }
        public new DateTime CreatedAt { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required AlertSeverity Severity { get; set; }
        public required string TitleTemplate { get; set; }
        public required string MessageTemplate { get; set; }
        public Dictionary<string, string> Parameters { get; set; } = new();
    }

    public class NotificationTemplate : BaseEntity
    {
        public new int Id { get; set; }
        public new DateTime CreatedAt { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required AlertSeverity Severity { get; set; }
        public required string TitleTemplate { get; set; }
        public required string MessageTemplate { get; set; }
        public Dictionary<string, string> Parameters { get; set; } = new();
    }

    public class NotificationSettings
    {
        public required int Id { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required int VehicleId { get; set; }
        public required decimal StabilityThreshold { get; set; }
        public required decimal SafetyThreshold { get; set; }
        public AlertSeverity MinimumSeverity { get; set; }
        public required bool EnableNotifications { get; set; }
        public required bool EnableEmailNotifications { get; set; }
        public required bool EnablePushNotifications { get; set; }
    }
} 
