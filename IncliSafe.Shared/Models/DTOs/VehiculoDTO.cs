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
            UltimaInspeccion = entity.UltimaInspeccion
        };
    }
} 