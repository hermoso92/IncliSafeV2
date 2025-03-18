using IncliSafe.Shared.Models.Enums;
using System;

namespace IncliSafe.Shared.Models.Entities
{
    public class License
    {
        public required int Id { get; set; }
        public required int VehicleId { get; set; }
        public required string LicenseNumber { get; set; } = string.Empty;
        public required DateTime IssueDate { get; set; }
        public required DateTime ExpirationDate { get; set; }
        public required bool IsActive { get; set; }
        public required LicenseType Type { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required string LicenseKey { get; set; } = string.Empty;
        public required string CompanyName { get; set; } = string.Empty;
        public required string Email { get; set; } = string.Empty;
        
        public virtual Vehiculo Vehicle { get; set; } = null!;
    }

    public enum LicenseType
    {
        Standard,
        Professional,
        Commercial,
        Special
    }
} 

