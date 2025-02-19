using System;
using System.ComponentModel.DataAnnotations;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafe.Shared.Models.DTOs
{
    public class VehiculoDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La placa es obligatoria")]
        [StringLength(20)]
        public string Placa { get; set; } = string.Empty;

        [Required(ErrorMessage = "La marca es obligatoria")]
        [StringLength(50)]
        public string Marca { get; set; } = string.Empty;

        [Required(ErrorMessage = "El modelo es obligatorio")]
        public string Modelo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El color es obligatorio")]
        [StringLength(50)]
        public string Color { get; set; } = string.Empty;

        [Required(ErrorMessage = "El año es obligatorio")]
        [Range(1900, 2100)]
        public int Año { get; set; }

        public string Tipo { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public int OwnerId { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UltimaInspeccion { get; set; }
        public string UserId { get; set; } = string.Empty;

        public static VehiculoDTO FromEntity(Vehiculo entity) => new()
        {
            Id = entity.Id,
            OwnerId = entity.OwnerId,
            Nombre = entity.Nombre,
            Placa = entity.Placa,
            Marca = entity.Marca,
            Modelo = entity.Modelo,
            Color = entity.Color,
            Año = entity.Año,
            Tipo = entity.Tipo,
            Estado = entity.Estado,
            Activo = entity.Activo,
            CreatedAt = entity.CreatedAt,
            UltimaInspeccion = entity.UltimaInspeccion,
            UserId = entity.UserId
        };
    }

    public static class VehiculoDTOExtensions
    {
        public static VehiculoDTO ToDTO(this Vehiculo vehiculo)
        {
            return new VehiculoDTO
            {
                Id = vehiculo.Id,
                OwnerId = vehiculo.OwnerId,
                Nombre = vehiculo.Nombre,
                Placa = vehiculo.Placa,
                Marca = vehiculo.Marca,
                Modelo = vehiculo.Modelo,
                Color = vehiculo.Color,
                Año = vehiculo.Año,
                Tipo = vehiculo.Tipo,
                Estado = vehiculo.Estado,
                Activo = vehiculo.Activo,
                CreatedAt = vehiculo.CreatedAt,
                UltimaInspeccion = vehiculo.UltimaInspeccion,
                UserId = vehiculo.UserId
            };
        }

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
                CreatedAt = dto.CreatedAt,
                UltimaInspeccion = dto.UltimaInspeccion,
                UserId = dto.UserId
            };
        }
    }
} 