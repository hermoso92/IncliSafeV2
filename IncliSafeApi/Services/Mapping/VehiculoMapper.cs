using IncliSafe.Shared.Models.DTOs;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafeApi.Services.Mapping
{
    public static class VehiculoMapper
    {
        public static VehicleDTO ToDTO(this Vehiculo vehicle)
        {
            return new VehicleDTO
            {
                Id = vehicle.Id,
                Nombre = vehicle.Nombre,
                Placa = vehicle.Placa,
                Marca = vehicle.Marca,
                Modelo = vehicle.Modelo,
                Color = vehicle.Color,
                Año = vehicle.Año,
                Tipo = vehicle.Tipo,
                Estado = vehicle.Estado,
                OwnerId = vehicle.OwnerId,
                Activo = vehicle.Activo,
                CreatedAt = vehicle.CreatedAt,
                UltimaInspeccion = vehicle.UltimaInspeccion
            };
        }

        public static Vehiculo ToEntity(this VehicleDTO dto)
        {
            return new Vehiculo
            {
                Id = dto.Id,
                Nombre = dto.Nombre,
                Placa = dto.Placa,
                Marca = dto.Marca,
                Modelo = dto.Modelo,
                Color = dto.Color,
                Año = dto.Año,
                Tipo = dto.Tipo,
                Estado = dto.Estado,
                OwnerId = dto.OwnerId,
                Activo = dto.Activo,
                CreatedAt = dto.CreatedAt,
                UltimaInspeccion = dto.UltimaInspeccion
            };
        }
    }
} 