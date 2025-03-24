using System;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Entities
{
    public class License
    {
        public int Id { get; set; }
        public required string LicenseNumber { get; set; }
        public LicenseType Type { get; set; }
        public required LicenseStatus Status { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int UserId { get; set; }
        public DateTime LastRenewal { get; set; }
        public required string Notes { get; set; }
    }
} 

