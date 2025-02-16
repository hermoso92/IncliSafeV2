using System;

namespace IncliSafe.Shared.Models
{
    public class LicenseStatus
    {
        public bool IsActive { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime ExpirationDate { get; set; }
    }
} 