using System.ComponentModel.DataAnnotations;
using IncliSafe.Shared.Models.Common;

namespace IncliSafe.Shared.Models.Entities
{
    public class Vehiculo : BaseEntity
    {
        [Required]
        public string Placa { get; set; } = string.Empty;
        
        [Required]
        public string Modelo { get; set; } = string.Empty;
        
        public int Año { get; set; }
        
        [Required]
        public string Marca { get; set; } = string.Empty;
        
        public string? Color { get; set; }
        
        public bool Activo { get; set; } = true;
        
        public virtual ICollection<License> Licenses { get; set; } = new List<License>();
        public virtual ICollection<VehicleAlert> Alerts { get; set; } = new List<VehicleAlert>();
        public virtual ICollection<Prediction> Predictions { get; set; } = new List<Prediction>();
    }
} 