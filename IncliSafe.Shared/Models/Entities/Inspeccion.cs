using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;

namespace IncliSafe.Shared.Models.Entities
{
    public class Inspeccion : BaseEntity
    {
        public required int VehicleId { get; set; }
        public required DateTime FechaInspeccion { get; set; }
        public required string Inspector { get; set; }
        public required string Resultado { get; set; }
        public required string Observaciones { get; set; }
        public decimal Score { get; set; }
        public required bool Aprobada { get; set; }
        public required string UbicacionInspeccion { get; set; }
        public required decimal CostoInspeccion { get; set; }
        public DateTime? FechaProximaInspeccion { get; set; }
        public virtual Vehicle Vehicle { get; set; } = null!;
        public virtual ICollection<Mantenimiento> Mantenimientos { get; set; } = new List<Mantenimiento>();
    }
} 

