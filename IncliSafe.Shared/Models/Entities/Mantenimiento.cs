using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;

namespace IncliSafe.Shared.Models.Entities
{
    public class Mantenimiento : BaseEntity
    {
        public required int VehicleId { get; set; }
        public required DateTime FechaMantenimiento { get; set; }
        public required string TipoMantenimiento { get; set; }
        public required string Descripcion { get; set; }
        public required string Tecnico { get; set; }
        public decimal Costo { get; set; }
        public virtual Vehicle Vehicle { get; set; } = null!;
        public virtual ICollection<Inspeccion> Inspecciones { get; set; } = new List<Inspeccion>();
    }
} 