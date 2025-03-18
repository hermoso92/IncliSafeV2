using IncliSafe.Shared.Models.DTOs;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafeApi.Services.Mapping
{
    public static class VehicleDTOExtensions
    {
        public static Vehiculo ToEntity(this VehiculoDTO dto)
        {
            return new Vehiculo
            {
                Id = dto.Id,
                OwnerId = dto.OwnerId,
                Nombre = dto.Nombre,
                Placa = dto.Placa,
                Marca = dto.Marca,
                Modelo = dto.Modelo,
                Color = dto.Color,
                Año = dto.Año,
                Tipo = dto.Tipo,
                Estado = dto.Estado,
                Activo = dto.Activo,
                UltimaInspeccion = dto.UltimaInspeccion
            };
        }

        public static VehiculoDTO ToDTO(this Vehiculo entity)
        {
            return VehiculoDTO.FromEntity(entity);
        }
    }
} 