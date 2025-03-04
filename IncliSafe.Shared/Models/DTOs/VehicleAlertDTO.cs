using System;
using IncliSafe.Shared.Models.Analysis;

namespace IncliSafe.Shared.Models.DTOs
{
    public class VehicleAlertDTO
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public Analysis.AlertType Type { get; set; }
        public Analysis.AlertSeverity Severity { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }
    }
} 