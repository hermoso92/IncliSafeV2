using IncliSafe.Shared.Models.Enums;
using System;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafe.Shared.Models.DTOs
{
    public class InspeccionDTO
    {
        public required int Id { get; set; }
        public required int VehicleId { get; set; }
        public required DateTime FechaInspeccion { get; set; }
        public required VehicleDTO Vehicle { get; set; } = null!;
        public required InspectionStatus Resultado { get; set; }
        public required string Observaciones { get; set; } = string.Empty;
        public required bool Aprobada { get; set; }
        public required string Inspector { get; set; } = string.Empty;
        public required string UbicacionInspeccion { get; set; } = string.Empty;
        public required decimal CostoInspeccion { get; set; }
        public DateTime? FechaProximaInspeccion { get; set; }

        public static InspeccionDTO FromEntity(Inspeccion entity)
        {
            return new InspeccionDTO
            {
                Id = entity.Id,
                FechaInspeccion = entity.FechaInspeccion,
                Resultado = entity.Resultado,
                Observaciones = entity.Observaciones,
                Aprobada = entity.Aprobada,
                Inspector = entity.Inspector,
                UbicacionInspeccion = entity.UbicacionInspeccion,
                CostoInspeccion = entity.CostoInspeccion,
                FechaProximaInspeccion = entity.FechaProximaInspeccion,
                VehicleId = entity.VehicleId,
                Vehicle = VehicleDTO.FromEntity(entity.Vehicle)
            };
        }

        public Inspeccion ToEntity()
        {
            return new Inspeccion
            {
                Id = Id,
                FechaInspeccion = FechaInspeccion,
                Resultado = Resultado,
                Observaciones = Observaciones,
                Aprobada = Aprobada,
                Inspector = Inspector,
                UbicacionInspeccion = UbicacionInspeccion,
                CostoInspeccion = CostoInspeccion,
                FechaProximaInspeccion = FechaProximaInspeccion,
                VehicleId = VehicleId
            };
        }
    }
} 

