using System;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Shared.Models.Patterns;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Entities
{
    public class VehicleAlert
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
        
        public virtual Vehiculo Vehicle { get; set; } = null!;
    }
} 
