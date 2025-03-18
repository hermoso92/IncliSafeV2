using System;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Enums;
using System.Collections.Generic;

namespace IncliSafe.Shared.Models.Core
{
    public class SensorReading : BaseEntity
    {
        public required string SensorId { get; set; }
        public required string Value { get; set; }
        public required string Unit { get; set; }
        public required DateTime Timestamp { get; set; }
        public string? Quality { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public required SensorType Type { get; set; }
        public required SensorStatus Status { get; set; }
        public string? Notes { get; set; }
        public Dictionary<string, string> Metadata { get; set; } = new();
    }

    public enum SensorType
    {
        Accelerometer,
        Gyroscope,
        Temperature,
        Pressure,
        Inclination,
        Custom
    }

    public enum SensorStatus
    {
        Normal,
        Warning,
        Error,
        Calibrating,
        Offline
    }
} 

