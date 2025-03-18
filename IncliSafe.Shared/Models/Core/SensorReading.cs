using IncliSafe.Shared.Models.Enums;
using System;
using System.Collections.Generic;

namespace IncliSafe.Shared.Models.Core
{
    public class SensorReading : BaseEntity
    {
        public required int SensorId { get; set; }
        public required DateTime Timestamp { get; set; }
        public required double Value { get; set; }
        public string? Unit { get; set; }
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

