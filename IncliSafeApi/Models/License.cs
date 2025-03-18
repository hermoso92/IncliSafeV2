using System;
using System.ComponentModel.DataAnnotations;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafeApi.Models
{
    public class License
    {
        public int Id { get; set; }
        public string LicenseKey { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public DateTime ActivationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public LicenseType Type { get; set; }
        [Required]
        public string Status { get; set; } = "Active";
        public bool IsActive { get; set; }
        public int? VehiculoId { get; set; }
        public virtual Vehiculo? Vehiculo { get; set; }
    }
} 