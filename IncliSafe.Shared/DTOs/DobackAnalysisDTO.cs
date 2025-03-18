using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Shared.Models.Core;

namespace IncliSafe.Shared.DTOs
{
    public class DobackAnalysisDTO
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public AnalysisStatus Status { get; set; }
        public List<Models.Core.SensorReading> Readings { get; set; } = new();
        public List<AnalysisPattern> Patterns { get; set; } = new();
        public List<AnalysisAlert> Alerts { get; set; } = new();
    }
}