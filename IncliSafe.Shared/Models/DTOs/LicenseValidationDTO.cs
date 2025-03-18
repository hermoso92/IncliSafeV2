using IncliSafe.Shared.Models.Enums;
using System;

namespace IncliSafe.Shared.Models.DTOs
{
    public class LicenseValidationDTO
    {
        public required string LicenseKey { get; set; }
        public required bool IsActive { get; set; }
        public required DateTime ExpirationDate { get; set; }
        
        public bool ValidateLicense()
        {
            if (string.IsNullOrEmpty(LicenseKey))
            {
                return false;
            }
            
            if (!IsActive)
            {
                return false;
            }
            
            if (ExpirationDate < DateTime.UtcNow)
            {
                return false;
            }
            
            return true;
        }
    }
} 

