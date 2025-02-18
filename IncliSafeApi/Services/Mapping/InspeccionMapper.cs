using IncliSafe.Shared.Models.DTOs;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafeApi.Services.Mapping
{
    public static class InspeccionMapper
    {
        public static InspeccionDTO ToDTO(this Inspeccion inspeccion)
        {
            return new InspeccionDTO
            {
                Id = inspeccion.Id,
                VehiculoId = inspeccion.VehiculoId,
                Fecha = inspeccion.Fecha,
                Observaciones = inspeccion.Observaciones,
                Estado = inspeccion.Estado
            };
        }

        public static Inspeccion ToEntity(this InspeccionDTO dto)
        {
            return new Inspeccion
            {
                Id = dto.Id,
                VehiculoId = dto.VehiculoId,
                Fecha = dto.Fecha,
                Observaciones = dto.Observaciones,
                Estado = dto.Estado
            };
        }
    }
} 