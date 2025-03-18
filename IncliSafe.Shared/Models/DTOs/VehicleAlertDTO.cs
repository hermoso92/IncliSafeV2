using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Shared.Models.Patterns;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.DTOs
{
    public class VehicleAlertDTO
    {
        public required int Id { get; set; }
        public required int VehicleId { get; set; }
        public required string Title { get; set; }
        public required string Message { get; set; }
        public Enums.AlertType Type { get; set; }
        public Enums.AlertSeverity Severity { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }
        public required bool IsAcknowledged { get; set; }
        public string? AcknowledgedBy { get; set; }
        public DateTime? AcknowledgedAt { get; set; }
        public TrendInfo? RelatedTrend { get; set; }
        public Patterns.DetectedPattern? RelatedPattern { get; set; }

        public static VehicleAlertDTO FromEntity(VehicleAlert alert)
        {
            return new VehicleAlertDTO
            {
                Id = alert.Id,
                VehicleId = alert.VehicleId,
                Title = alert.Title,
                Message = alert.Message,
                Type = alert.Type,
                Severity = alert.Severity,
                CreatedAt = alert.CreatedAt,
                IsRead = alert.IsRead,
                ReadAt = alert.ReadAt,
                IsAcknowledged = alert.IsAcknowledged,
                AcknowledgedBy = alert.AcknowledgedBy,
                AcknowledgedAt = alert.AcknowledgedAt,
                RelatedTrend = alert.RelatedTrend,
                RelatedPattern = alert.RelatedPattern
            };
        }

        public VehicleAlert ToEntity()
        {
            return new VehicleAlert
            {
                Id = Id,
                VehicleId = VehicleId,
                Title = Title,
                Message = Message,
                Type = Type,
                Severity = Severity,
                CreatedAt = CreatedAt,
                IsRead = IsRead,
                ReadAt = ReadAt,
                IsAcknowledged = IsAcknowledged,
                AcknowledgedBy = AcknowledgedBy,
                AcknowledgedAt = AcknowledgedAt,
                RelatedTrend = RelatedTrend,
                RelatedPattern = RelatedPattern
            };
        }
    }
} 
