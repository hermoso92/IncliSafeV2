using System;

namespace IncliSafe.Shared.Models.Core
{
    public class SensorReading
    {
        public int Id { get; set; }
        public int SensorId { get; set; }
        public DateTime Timestamp { get; set; }
        public double Value { get; set; }
        public required string Unit { get; set; }
        public bool IsValid { get; set; }
        public required string Notes { get; set; }
    }
} 

