using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IncliSafe.Shared.Models.Notifications;

namespace IncliSafe.Shared.Models.Entities
{
    public class Vehiculo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La placa es obligatoria")]
        [StringLength(20)]
        public string Placa { get; set; } = string.Empty;

        [Required(ErrorMessage = "La marca es obligatoria")]
        [StringLength(50)]
        public string Marca { get; set; } = string.Empty;

        [Required(ErrorMessage = "El modelo es obligatorio")]
        [StringLength(50)]
        public string Modelo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El color es obligatorio")]
        [StringLength(50)]
        public string Color { get; set; } = string.Empty;

        [Required(ErrorMessage = "El año es obligatorio")]
        [Range(1900, 2100)]
        public int Año { get; set; }

        public VehicleType Tipo { get; set; }
        public string Estado { get; set; } = string.Empty;
        public int UserId { get; set; }
        public virtual Usuario? Usuario { get; set; }
        public virtual ICollection<Inspeccion> Inspecciones { get; set; } = new List<Inspeccion>();
        public DateTime UltimaInspeccion { get; set; }
        public virtual ICollection<License> Licenses { get; set; } = new List<License>();
        public virtual NotificationSettings NotificationSettings { get; set; } = new();
    }
} 