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
        public required string Description { get; set; }
        public required string Type { get; set; }
        public required string Status { get; set; }
        public required bool IsActive { get; set; }
        public required string Placa { get; set; }
        public required string Marca { get; set; }
        public required string Modelo { get; set; }
        public required int Año { get; set; }
        public required string Color { get; set; }
        public required string VIN { get; set; }
        public required string Estado { get; set; }
        public required int PropietarioId { get; set; }
        public required string PropietarioNombre { get; set; }
        public DateTime? UltimaInspeccion { get; set; }
        public DateTime? ProximaInspeccion { get; set; }
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
        public virtual Usuario Propietario { get; set; } = null!;
        public virtual ICollection<Inspeccion> Inspecciones { get; set; } = new List<Inspeccion>();
        public virtual ICollection<Mantenimiento> Mantenimientos { get; set; } = new List<Mantenimiento>();
        public virtual ICollection<SensorReading> SensorReadings { get; set; } = new List<SensorReading>();
        public virtual ICollection<Prediction> Predictions { get; set; } = new List<Prediction>();
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public virtual ICollection<NotificationSettings> NotificationSettings { get; set; } = new List<NotificationSettings>();
    }
}
