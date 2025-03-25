using IncliSafe.Shared.Models.Enums;
using System;

namespace IncliSafe.Shared.Models.DTOs
{
    public class AlertDTO
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public NotificationSeverity Severity { get; set; }
        public AlertCategory Category { get; set; }
        public string Source { get; set; }
        public AlertStatus Status { get; set; }
        public bool IsAcknowledged { get; set; }
        public DateTime? AcknowledgedAt { get; set; }
        public string AcknowledgedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public string Resolution { get; set; }
        public string ResolutionDetails { get; set; }
        public string AssignedTo { get; set; }
        public NotificationPriority Priority { get; set; }
        public string Tags { get; set; }
        public string RelatedEntities { get; set; }
        public string ActionRequired { get; set; }
        public string Impact { get; set; }
        public RiskLevel RiskLevel { get; set; }
        public ComplianceStatus ComplianceStatus { get; set; }
        public string Documentation { get; set; }
        public string Notes { get; set; }
    }
} 