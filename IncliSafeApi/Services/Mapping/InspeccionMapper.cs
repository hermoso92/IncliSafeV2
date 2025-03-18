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
                VehicleId = inspeccion.VehicleId,
                FechaInspeccion = inspeccion.FechaInspeccion,
                Resultado = inspeccion.Resultado,
                Observaciones = inspeccion.Observaciones,
                Aprobada = inspeccion.Aprobada,
                Inspector = inspeccion.Inspector,
                UbicacionInspeccion = inspeccion.UbicacionInspeccion,
                CostoInspeccion = inspeccion.CostoInspeccion,
                FechaProximaInspeccion = inspeccion.FechaProximaInspeccion,
                Vehicle = inspeccion.Vehicle != null ? VehicleDTO.FromEntity(inspeccion.Vehicle) : null
            };
        }

        public static Inspeccion ToEntity(this InspeccionDTO dto)
        {
            return new Inspeccion
            {
                Id = dto.Id,
                VehicleId = dto.VehicleId,
                FechaInspeccion = dto.FechaInspeccion,
                Resultado = dto.Resultado,
                Observaciones = dto.Observaciones,
                Aprobada = dto.Aprobada,
                Inspector = dto.Inspector,
                UbicacionInspeccion = dto.UbicacionInspeccion,
                CostoInspeccion = dto.CostoInspeccion,
                FechaProximaInspeccion = dto.FechaProximaInspeccion
            };
        }
    }
} 