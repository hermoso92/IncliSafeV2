using IncliSafe.Shared.Models.Enums;
using System;

namespace IncliSafe.Shared.Models.DTOs
{
    public class VehicleSummaryDTO
    {
        public required int Id { get; set; }
        public required string Nombre { get; set; } = string.Empty;
        public required string Placa { get; set; } = string.Empty;
        public required int UserId { get; set; }
        public required decimal StabilityScore { get; set; }
        public required decimal SafetyScore { get; set; }
        public required string Estado { get; set; } = string.Empty;
        public DateTime? UltimaInspeccion { get; set; }
        public required decimal UltimoStabilityScore { get; set; }
        public required decimal UltimoSafetyScore { get; set; }
        public bool RequiereInspeccion => !UltimaInspeccion.HasValue || 
            UltimaInspeccion.Value.AddMonths(3) < DateTime.UtcNow;
        public bool RequiereMantenimiento => UltimoStabilityScore < 0.7M || 
            UltimoSafetyScore < 0.7M;
        public required LicenseValidationDTO LicenseStatus { get; set; } = null!;
    }
} 

