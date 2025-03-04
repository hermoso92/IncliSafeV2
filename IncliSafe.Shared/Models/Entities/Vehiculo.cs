using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IncliSafe.Shared.Models.Notifications;
using System.Linq;
using IncliSafe.Shared.Models.Analysis.Core;

namespace IncliSafe.Shared.Models.Entities
{
    public class Vehiculo
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
        
        public virtual Usuario Owner { get; set; } = null!;
        public virtual License License { get; set; } = null!;
        public virtual ICollection<Inspeccion> Inspecciones { get; set; } = new List<Inspeccion>();
        public virtual ICollection<DobackAnalysis> DobackAnalyses { get; set; } = new List<DobackAnalysis>();
        public virtual ICollection<Alert> Alerts { get; set; } = new List<Alert>();
        public virtual ICollection<VehicleMetrics> Metrics { get; set; } = new List<VehicleMetrics>();
        public virtual NotificationSettings NotificationSettings { get; set; } = new();
        public virtual AlertSettings AlertSettings { get; set; } = null!;
        public virtual ICollection<TrendAnalysis> TrendAnalyses { get; set; } = new List<TrendAnalysis>();
    }
} 