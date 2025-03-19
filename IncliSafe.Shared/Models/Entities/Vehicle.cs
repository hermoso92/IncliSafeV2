using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Notifications;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Entities
{
    public class Vehicle : BaseEntity
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string LicensePlate { get; set; }
        public required string Model { get; set; }
        public required string Manufacturer { get; set; }
        public required int Year { get; set; }
        public required IncliSafe.Shared.Models.Enums.VehicleType Type { get; set; }
        public required IncliSafe.Shared.Models.Enums.VehicleStatus Status { get; set; }
        public required IncliSafe.Shared.Models.Enums.VehicleCondition Condition { get; set; }
        public required IncliSafe.Shared.Models.Enums.FuelType FuelType { get; set; }
        public required decimal Mileage { get; set; }
        public required int OwnerId { get; set; }
        public required DateTime PurchaseDate { get; set; }
        public required DateTime LastMaintenanceDate { get; set; }
        public required DateTime NextMaintenanceDate { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = new();
        public List<decimal> DataPoints { get; set; } = new();
        public List<DateTime> Timestamps { get; set; } = new();
        public Dictionary<string, object> Metadata { get; set; } = new();
        public required string VIN { get; set; }
        public required bool IsActive { get; set; }
        public virtual Usuario Owner { get; set; } = null!;
        public virtual ICollection<Mantenimiento> Mantenimientos { get; set; } = new List<Mantenimiento>();
        public List<Alert> Alertas { get; set; } = new();
        public decimal Score { get; set; }
        public required string NumeroSerie { get; set; }
        public required string NumeroMotor { get; set; }
        public decimal CapacidadCarga { get; set; }
        public decimal PesoVacio { get; set; }
        public decimal PesoTotal { get; set; }
        public decimal LongitudTotal { get; set; }
        public decimal AnchoTotal { get; set; }
        public decimal AlturaTotal { get; set; }
        public decimal DistanciaEjes { get; set; }
        public decimal DistanciaEjePosterior { get; set; }
        public decimal VelocidadMaxima { get; set; }
        public virtual ICollection<Inspeccion> Inspecciones { get; set; } = new List<Inspeccion>();
        public virtual ICollection<SensorReading> SensorReadings { get; set; } = new List<SensorReading>();
        public virtual ICollection<Prediction> Predictions { get; set; } = new List<Prediction>();
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public virtual ICollection<NotificationSettings> NotificationSettings { get; set; } = new List<NotificationSettings>();
    }
}
