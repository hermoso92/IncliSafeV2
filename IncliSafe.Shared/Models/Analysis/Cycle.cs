using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafe.Shared.Models.Analysis
{
    public class Cycle
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal Duration { get; set; }
        public decimal Distance { get; set; }
        public decimal AverageSpeed { get; set; }
        public decimal MaxSpeed { get; set; }
        public decimal StabilityScore { get; set; }
        public decimal SafetyScore { get; set; }
        public decimal MaintenanceScore { get; set; }
        public virtual Vehiculo Vehicle { get; set; } = null!;
        public virtual List<DobackData> Data { get; set; } = new();
    }
} 