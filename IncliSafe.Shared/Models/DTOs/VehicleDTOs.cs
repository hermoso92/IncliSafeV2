using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Shared.Models.Analysis.Core;
using IncliSafe.Shared.Models.Patterns;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.DTOs
{
    public class VehicleDTO
    {
        public required int Id { get; set; }
        public required string Placa { get; set; }
        public required string Marca { get; set; }
        public required string Modelo { get; set; }
        public int Año { get; set; }
        public required string Color { get; set; }
        public required string VIN { get; set; }
        public required string Estado { get; set; }
        public required int PropietarioId { get; set; }
        public required string PropietarioNombre { get; set; }
        public required DateTime UltimaInspeccion { get; set; }
        public required DateTime ProximaInspeccion { get; set; }
        public List<string> Alertas { get; set; } = new();
        public required double Score { get; set; }
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
        public required VehicleStatus Status { get; set; }
        public required VehicleType Type { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required bool IsActive { get; set; }

        public static VehicleDTO FromEntity(Vehicle entity)
        {
            return new VehicleDTO
            {
                Id = entity.Id,
                Placa = entity.Placa,
                Marca = entity.Marca,
                Modelo = entity.Modelo,
                Año = entity.Año,
                Color = entity.Color ?? string.Empty,
                NumeroSerie = entity.NumeroSerie ?? string.Empty,
                NumeroMotor = entity.NumeroMotor ?? string.Empty,
                CapacidadCarga = entity.CapacidadCarga,
                PesoVacio = entity.PesoVacio,
                PesoTotal = entity.PesoTotal,
                LongitudTotal = entity.LongitudTotal,
                AnchoTotal = entity.AnchoTotal,
                AlturaTotal = entity.AlturaTotal,
                DistanciaEjes = entity.DistanciaEjes,
                DistanciaEjePosterior = entity.DistanciaEjePosterior,
                VelocidadMaxima = entity.VelocidadMaxima,
                Status = entity.Status,
                Type = entity.Type,
                CreatedAt = entity.CreatedAt,
                IsActive = true,
                VIN = string.Empty,
                Estado = "Activo",
                PropietarioNombre = "Sin Asignar"
            };
        }

        public Vehicle ToEntity()
        {
            return new Vehicle
            {
                Id = Id,
                Placa = Placa,
                Marca = Marca,
                Modelo = Modelo,
                Año = Año,
                Color = Color,
                NumeroSerie = NumeroSerie,
                NumeroMotor = NumeroMotor,
                CapacidadCarga = CapacidadCarga,
                PesoVacio = PesoVacio,
                PesoTotal = PesoTotal,
                LongitudTotal = LongitudTotal,
                AnchoTotal = AnchoTotal,
                AlturaTotal = AlturaTotal,
                DistanciaEjes = DistanciaEjes,
                DistanciaEjePosterior = DistanciaEjePosterior,
                VelocidadMaxima = VelocidadMaxima,
                Status = Status,
                Type = Type,
                CreatedAt = CreatedAt
            };
        }
    }

    public class VehicleDetailsDTO : VehicleDTO
    {
        public List<InspeccionDTO> Inspecciones { get; set; } = new();
        public List<MantenimientoDTO> Mantenimientos { get; set; } = new();
        public List<SensorReadingDTO> SensorReadings { get; set; } = new();
        public List<VehicleMetricsDTO> Metrics { get; set; } = new();
    }

    public class MantenimientoDTO
    {
        public required int Id { get; set; }
        public required int VehicleId { get; set; }
        public required string Descripcion { get; set; }
        public required DateTime FechaProgramada { get; set; }
        public DateTime? FechaRealizada { get; set; }
        public required MaintenanceType Type { get; set; }
        public required MaintenancePriority Priority { get; set; }
        public required decimal Cost { get; set; }
        public required string Notes { get; set; } = string.Empty;
    }

    public enum MaintenanceType
    {
        Preventive,
        Corrective,
        Scheduled,
        Emergency,
        Inspection,
        Upgrade
    }

    public enum MaintenancePriority
    {
        Low,
        Medium,
        High,
        Critical
    }

    public class VehicleMaintenanceDTO
    {
        public required int Id { get; set; }
        public required int VehicleId { get; set; }
        public required string Description { get; set; }
        public required DateTime ScheduledDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public required MaintenanceType Type { get; set; }
        public required MaintenancePriority Priority { get; set; }
        public required decimal Cost { get; set; }
        public required string Notes { get; set; } = string.Empty;
    }

    public class VehicleMetricsDTO
    {
        public required int Id { get; set; }
        public required int VehicleId { get; set; }
        public required decimal AverageStabilityScore { get; set; }
        public required decimal AverageSafetyScore { get; set; }
        public required decimal AverageMaintenanceScore { get; set; }
        public required decimal AveragePerformanceScore { get; set; }
        public required decimal AverageEfficiencyScore { get; set; }
        public required DateTime LastUpdated { get; set; }
    }

    public class VehicleSettingsDTO
    {
        public required int VehicleId { get; set; }
        public required decimal StabilityThreshold { get; set; }
        public required decimal SafetyThreshold { get; set; }
        public required decimal MaintenanceThreshold { get; set; }
        public required bool EnableNotifications { get; set; }
        public required bool EnableEmailNotifications { get; set; }
        public required bool EnablePushNotifications { get; set; }
        public required NotificationSeverity MinimumSeverity { get; set; }
    }

    public class SensorReadingDTO
    {
        public required int Id { get; set; }
        public required int VehicleId { get; set; }
        public required DateTime Timestamp { get; set; }
        public required decimal Value { get; set; }
        public required string SensorType { get; set; } = string.Empty;
        public required string Unit { get; set; } = string.Empty;
        public required bool IsValid { get; set; }
        public string? Notes { get; set; }
    }
} 
