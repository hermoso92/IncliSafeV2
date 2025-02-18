using System;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafe.Shared.Models.DTOs
{
    public class LicenseDTO
    {
        public int Id { get; set; }
        public LicenseType Type { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsExpired => ExpirationDate < DateTime.UtcNow;
        public int DaysUntilExpiration => (int)(ExpirationDate - DateTime.UtcNow).TotalDays;
    }
} 