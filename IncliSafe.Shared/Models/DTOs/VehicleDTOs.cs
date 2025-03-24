using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Shared.Models.Analysis.Core;
using IncliSafe.Shared.Models.Patterns;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.DTOs
{
    public class VehicleDTO : BaseDTO
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string LicensePlate { get; set; }
        public required string Manufacturer { get; set; }
        public required string Model { get; set; }
        public required int Year { get; set; }
        public required string VIN { get; set; }
        public required FuelType FuelType { get; set; }
        public required VehicleCondition Condition { get; set; }
        public required decimal Mileage { get; set; }
        public required decimal CapacidadCarga { get; set; }
        public required decimal PesoVacio { get; set; }
        public required decimal PesoTotal { get; set; }
        public required decimal LongitudTotal { get; set; }
        public required decimal AnchoTotal { get; set; }
        public required decimal AlturaTotal { get; set; }
        public required decimal DistanciaEjes { get; set; }
        public required decimal DistanciaEjePosterior { get; set; }
        public required decimal VelocidadMaxima { get; set; }
        public required DateTime LastMaintenanceDate { get; set; }
        public required DateTime NextMaintenanceDate { get; set; }
        public required DateTime PurchaseDate { get; set; }
        public required int OwnerId { get; set; }
        public required bool IsActive { get; set; }
        public required string NumeroSerie { get; set; }
        public required string NumeroMotor { get; set; }
        public required VehicleType Type { get; set; }
        public required VehicleStatus Status { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }

        public static VehicleDTO FromEntity(Vehicle entity)
        {
            return new VehicleDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                LicensePlate = entity.LicensePlate,
                Manufacturer = entity.Manufacturer,
                Model = entity.Model,
                Year = entity.Year,
                VIN = entity.VIN,
                FuelType = entity.FuelType,
                Condition = entity.Condition,
                Mileage = entity.Mileage,
                CapacidadCarga = entity.CapacidadCarga,
                PesoVacio = entity.PesoVacio,
                PesoTotal = entity.PesoTotal,
                LongitudTotal = entity.LongitudTotal,
                AnchoTotal = entity.AnchoTotal,
                AlturaTotal = entity.AlturaTotal,
                DistanciaEjes = entity.DistanciaEjes,
                DistanciaEjePosterior = entity.DistanciaEjePosterior,
                VelocidadMaxima = entity.VelocidadMaxima,
                LastMaintenanceDate = entity.LastMaintenanceDate,
                NextMaintenanceDate = entity.NextMaintenanceDate,
                PurchaseDate = entity.PurchaseDate,
                OwnerId = entity.OwnerId,
                IsActive = entity.IsActive,
                NumeroSerie = entity.NumeroSerie,
                NumeroMotor = entity.NumeroMotor,
                Type = entity.Type,
                Status = entity.Status,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                CreatedBy = entity.CreatedBy,
                UpdatedBy = entity.UpdatedBy,
                IsDeleted = entity.IsDeleted,
                DeletedAt = entity.DeletedAt,
                DeletedBy = entity.DeletedBy
            };
        }

        public Vehicle ToEntity()
        {
            return new Vehicle
            {
                Id = Id,
                Name = Name,
                Description = Description,
                LicensePlate = LicensePlate,
                Manufacturer = Manufacturer,
                Model = Model,
                Year = Year,
                VIN = VIN,
                FuelType = FuelType,
                Condition = Condition,
                Mileage = Mileage,
                CapacidadCarga = CapacidadCarga,
                PesoVacio = PesoVacio,
                PesoTotal = PesoTotal,
                LongitudTotal = LongitudTotal,
                AnchoTotal = AnchoTotal,
                AlturaTotal = AlturaTotal,
                DistanciaEjes = DistanciaEjes,
                DistanciaEjePosterior = DistanciaEjePosterior,
                VelocidadMaxima = VelocidadMaxima,
                LastMaintenanceDate = LastMaintenanceDate,
                NextMaintenanceDate = NextMaintenanceDate,
                PurchaseDate = PurchaseDate,
                OwnerId = OwnerId,
                IsActive = IsActive,
                NumeroSerie = NumeroSerie,
                NumeroMotor = NumeroMotor,
                Type = Type,
                Status = Status,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                CreatedBy = CreatedBy,
                UpdatedBy = UpdatedBy,
                IsDeleted = IsDeleted,
                DeletedAt = DeletedAt,
                DeletedBy = DeletedBy
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
