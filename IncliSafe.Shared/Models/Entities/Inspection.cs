using IncliSafe.Shared.Models.Enums;
using System;
using IncliSafe.Shared.Models.Common;

namespace IncliSafe.Shared.Models.Entities
{
    public class Inspection : BaseEntity
    {
        public required int VehicleId { get; set; }
        public required DateTime InspectionDate { get; set; }
        public required string Type { get; set; }
        public required string Result { get; set; }
        public required decimal Score { get; set; }
        public required string Observations { get; set; } = string.Empty;
        public required int InspectorId { get; set; }
        public required bool RequiresAction { get; set; }
        public DateTime? NextInspectionDate { get; set; }
        public virtual Vehicle Vehicle { get; set; } = null!;
        public virtual Usuario Inspector { get; set; } = null!;
    }
} 

