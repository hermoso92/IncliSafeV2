using System;
using System.Collections.Generic;

namespace IncliSafe.Shared.Models.Analysis.Core
{
    public class Anomaly
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal Value { get; set; }
        public decimal ExpectedValue { get; set; }
        public decimal Deviation { get; set; }
        public AnomalyType Type { get; set; }
        public List<string> PossibleCauses { get; set; } = new();
        public List<string> RecommendedActions { get; set; } = new();
        public int VehicleId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int? TrendAnalysisId { get; set; }

        public string GetSeverityLevel() => Type switch
        {
            AnomalyType.High => "Alto",
            AnomalyType.Low => "Bajo",
            AnomalyType.Pattern => "Patrón",
            AnomalyType.Seasonal => "Estacional",
            AnomalyType.Trend => "Tendencia",
            _ => "Desconocido"
        };

        public string GetStatusDisplay() => Status switch
        {
            "New" => "Nuevo",
            "InProgress" => "En Proceso",
            "Resolved" => "Resuelto",
            "Ignored" => "Ignorado",
            _ => Status
        };

        public bool RequiresImmediate() => 
            Type == AnomalyType.High || 
            Category == "Safety" || 
            Status == "New";
    }
} 