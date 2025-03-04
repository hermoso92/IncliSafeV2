using System;
using System.ComponentModel.DataAnnotations;

namespace IncliSafe.Shared.Models.Entities
{
    public class Inspeccion
    {
        public int Id { get; set; }
        public int VehiculoId { get; set; }
        public int InspectorId { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string Observaciones { get; set; } = string.Empty;
        public decimal Score { get; set; }
        
        public virtual Vehiculo Vehiculo { get; set; } = null!;
        public virtual Usuario Inspector { get; set; } = null!;
    }
} 