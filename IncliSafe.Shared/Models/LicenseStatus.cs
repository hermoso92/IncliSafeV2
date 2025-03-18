using IncliSafe.Shared.Models.Enums;
using System;

namespace IncliSafe.Shared.Models
{
    public class LicenseStatus
    {
        public required bool IsActive { get; set; }
        public required string Message { get; set; } = string.Empty;
        public required DateTime ExpirationDate { get; set; }
    }
} 

