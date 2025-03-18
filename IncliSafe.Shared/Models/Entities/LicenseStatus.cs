using IncliSafe.Shared.Models.Enums;
using System;

namespace IncliSafe.Shared.Models.Entities
{
    public class LicenseStatus
    {
        public required bool IsActive { get; set; }
        public required string Message { get; set; } = string.Empty;
        public required DateTime ExpirationDate { get; set; }
        public int DaysRemaining => (ExpirationDate - DateTime.UtcNow).Days;
        public string Status => IsActive ? "Activa" : "Inactiva";
    }
} 

