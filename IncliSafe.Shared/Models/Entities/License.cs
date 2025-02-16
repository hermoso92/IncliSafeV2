using System;

namespace IncliSafe.Shared.Models.Entities
{
    public class License
    {
        public int Id { get; set; }
        public string LicenseKey { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsActive { get; set; }
        public LicenseType Type { get; set; }
        public int VehiculoId { get; set; }
        public virtual Vehiculo? Vehiculo { get; set; }
    }
} 