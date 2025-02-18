using System;

namespace IncliSafe.Shared.Models.DTOs
{
    public class InspeccionDTO
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string Observaciones { get; set; } = string.Empty;
        public int VehiculoId { get; set; }
        public VehiculoDTO? Vehiculo { get; set; }

        public static InspeccionDTO FromEntity(Inspeccion entity)
        {
            return new InspeccionDTO
            {
                Id = entity.Id,
                Fecha = entity.Fecha,
                Estado = entity.Estado,
                Observaciones = entity.Observaciones,
                VehiculoId = entity.VehiculoId,
                Vehiculo = entity.Vehiculo != null ? VehiculoDTO.FromEntity(entity.Vehiculo) : null
            };
        }

        public Inspeccion ToEntity()
        {
            return new Inspeccion
            {
                Id = Id,
                Fecha = Fecha,
                Estado = Estado,
                Observaciones = Observaciones,
                VehiculoId = VehiculoId
            };
        }
    }
} 