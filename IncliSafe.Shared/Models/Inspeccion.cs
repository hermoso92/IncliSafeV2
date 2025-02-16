using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IncliSafe.Shared.Models
{
    public class Inspeccion
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El vehículo es obligatorio")]
        public int VehiculoId { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        [StringLength(50)]
        public string Estado { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Observaciones { get; set; }

        [ForeignKey(nameof(VehiculoId))]
        public virtual Vehiculo? Vehiculo { get; set; }

        public int? InspectorId { get; set; }

        [ForeignKey(nameof(InspectorId))]
        public virtual Usuario? Inspector { get; set; }

        public Inspeccion()
        {
            Fecha = DateTime.Today;
            Estado = "Pendiente";
            Observaciones = string.Empty;
        }
    }
} 