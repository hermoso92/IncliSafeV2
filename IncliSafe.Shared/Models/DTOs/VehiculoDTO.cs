using IncliSafe.Shared.Models.Enums;
using System;
using IncliSafe.Shared.Models.Common;

namespace IncliSafe.Shared.Models.DTOs
{
    public class VehiculoDTO
    {
        public required int Id { get; set; }
        public required string Placa { get; set; } = string.Empty;
        public required string Marca { get; set; } = string.Empty;
        public required string Modelo { get; set; } = string.Empty;
        public int Año { get; set; }
        public required string Color { get; set; } = string.Empty;
        public required string VIN { get; set; } = string.Empty;
        public required string Estado { get; set; } = string.Empty;
        public required int PropietarioId { get; set; }
        public required string PropietarioNombre { get; set; } = string.Empty;
        public required DateTime UltimaInspeccion { get; set; }
        public required DateTime ProximaInspeccion { get; set; }
        public List<Alert> Alertas { get; set; } = new();
        public required decimal Score { get; set; }
        public required string NumeroSerie { get; set; } = string.Empty;
        public required string NumeroMotor { get; set; } = string.Empty;
        public required decimal CapacidadCarga { get; set; }
        public required decimal PesoVacio { get; set; }
        public required decimal PesoTotal { get; set; }
        public required decimal LongitudTotal { get; set; }
        public required decimal AnchoTotal { get; set; }
        public required decimal AlturaTotal { get; set; }
        public required decimal DistanciaEjes { get; set; }
        public required decimal DistanciaEjePosterior { get; set; }
        public required decimal VelocidadMaxima { get; set; }
        public required string Status { get; set; } = string.Empty;
        public required string Type { get; set; } = string.Empty;
        public required DateTime CreatedAt { get; set; }
        public required bool IsActive { get; set; }
    }
} 



