using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafe.Shared.Models.Analysis
{
    public class TrendAnalysisEntity
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TrendValue { get; set; }
        public decimal Seasonality { get; set; }
        public decimal Correlation { get; set; }
        public List<TrendData> Data { get; set; } = new();
        public virtual Vehiculo Vehicle { get; set; } = null!;
    }
} 