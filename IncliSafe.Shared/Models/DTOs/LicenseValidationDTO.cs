using System;

namespace IncliSafe.Shared.Models.DTOs
{
    public class LicenseValidationDTO
    {
        public bool IsValid { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? ExpirationDate { get; set; }
        public int? DaysUntilExpiration { get; set; }
        public string Type { get; set; } = string.Empty;
        public string[] Warnings { get; set; } = Array.Empty<string>();

        public static LicenseValidationDTO FromLicense(LicenseDTO license)
        {
            var warnings = new List<string>();
            if (license.IsExpired)
                warnings.Add("La licencia ha expirado");
            if (license.DaysUntilExpiration <= 30)
                warnings.Add($"La licencia expirará en {license.DaysUntilExpiration} días");
            if (!license.IsActive)
                warnings.Add("La licencia está inactiva");

            return new LicenseValidationDTO
            {
                IsValid = license.IsActive && !license.IsExpired,
                Status = license.IsActive ? (license.IsExpired ? "Expirada" : "Activa") : "Inactiva",
                ExpirationDate = license.ExpirationDate,
                DaysUntilExpiration = license.DaysUntilExpiration,
                Type = license.Type.ToString(),
                Warnings = warnings.ToArray()
            };
        }
    }
} 