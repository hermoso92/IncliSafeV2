using System;

namespace IncliSafe.Shared.Models.Entities
{
    public class License
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public string LicenseNumber { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        public LicenseType Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public string LicenseKey { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        
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