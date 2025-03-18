using System;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis
{
    public class AnalysisAlert : BaseEntity
    {
        public override Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Message { get; set; }
        public required AlertSeverity Severity { get; set; }
        public required AlertType Type { get; set; }
        public required Guid VehicleId { get; set; }
        public required Guid AnalysisId { get; set; }
        public DateTime? ReadAt { get; set; }
        public bool IsRead => ReadAt.HasValue;
        public override DateTime CreatedAt { get; set; }
    }
} 