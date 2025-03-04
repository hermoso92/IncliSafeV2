using System;

namespace IncliSafe.Shared.Models.DTOs
{
    public class VehicleSummaryDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Placa { get; set; } = string.Empty;
        public int UserId { get; set; }
        public decimal StabilityScore { get; set; }
        public decimal SafetyScore { get; set; }
        public string Estado { get; set; } = string.Empty;
        public DateTime? UltimaInspeccion { get; set; }
        public decimal UltimoStabilityScore { get; set; }
        public decimal UltimoSafetyScore { get; set; }
        public bool RequiereInspeccion => !UltimaInspeccion.HasValue || 
            UltimaInspeccion.Value.AddMonths(3) < DateTime.UtcNow;
        public bool RequiereMantenimiento => UltimoStabilityScore < 0.7M || 
            UltimoSafetyScore < 0.7M;
        public LicenseValidationDTO LicenseStatus { get; set; } = null!;
    }
} 