using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.DTOs.Base;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.DTOs
{
    public class VehiculoDTO : BaseDTO
    {
        public override int Id { get; set; }
        public required string Placa { get; set; }
        public required string Marca { get; set; }
        public required string Modelo { get; set; }
        public int Año { get; set; }
        public required string Color { get; set; }
        public required string VIN { get; set; }
        public required VehicleType Tipo { get; set; }
        public required VehicleStatus Estado { get; set; }
        public required string PropietarioNombre { get; set; }
        public string? PropietarioDocumento { get; set; }
        public string? PropietarioTelefono { get; set; }
        public required string NumeroSerie { get; set; }
        public decimal? Kilometraje { get; set; }
        public decimal? CapacidadCarga { get; set; }
        public decimal? PesoBruto { get; set; }
        public decimal? PesoNeto { get; set; }
        public int? NumeroEjes { get; set; }
        public int? NumeroAsientos { get; set; }
        public string? Observaciones { get; set; }
        public DateTime? FechaFabricacion { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? UltimaInspeccion { get; set; }
        public DateTime? ProximaInspeccion { get; set; }
        public bool TieneSeguro { get; set; }
        public string? NumeroPoliza { get; set; }
        public string? CompañiaSeguro { get; set; }
        public DateTime? VencimientoSeguro { get; set; }
        public bool RequiereMantenimiento { get; set; }
        public DateTime? UltimoMantenimiento { get; set; }
        public DateTime? ProximoMantenimiento { get; set; }
        public required string NumeroMotor { get; set; }
        public FuelType? TipoCombustible { get; set; }
        public decimal? CapacidadTanque { get; set; }
        public VehicleCondition? Condicion { get; set; }
        public string? UbicacionActual { get; set; }
        public bool EstaActivo { get; set; }
        public string? Categoria { get; set; }
        public string? SubCategoria { get; set; }
        public string? Configuracion { get; set; }
        public string? CarroceriaTipo { get; set; }
        public required string LicenseNumber { get; set; }
        public DateTime? LicenseExpiry { get; set; }
        public required LicenseStatus Status { get; set; }
        public required LicenseType Type { get; set; }
        public required string IssuingAuthority { get; set; }
        public List<string> Alertas { get; set; } = new List<string>();
        public decimal Score { get; set; }
        public decimal LongitudTotal { get; set; }
        public decimal DistanciaEjePosterior { get; set; }
        public decimal VelocidadMaxima { get; set; }
    }

    public class LicenseDTO : BaseDTO
    {
        public int VehiculoId { get; set; }
        public required string LicenseNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public required string Status { get; set; }
        public required string Type { get; set; }
        public required string IssuingAuthority { get; set; }
        public List<string> Restrictions { get; set; } = new List<string>();
    }
}



