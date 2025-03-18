using IncliSafe.Shared.Models.Enums;
using System;

namespace IncliSafe.Shared.Models.DTOs
{
    public class DobackDataDto
    {
        public required double AccelerationX { get; set; }
        public required double AccelerationY { get; set; }
        public required double AccelerationZ { get; set; }
        public required double Roll { get; set; }
        public required double Pitch { get; set; }
        public required double Yaw { get; set; }
        public required double Speed { get; set; }
        public required double StabilityIndex { get; set; }
        public required double Temperature { get; set; }
        public required double Humidity { get; set; }
        public required double TimeAntWifi { get; set; }
        public required DateTime Timestamp { get; set; }
    }
} 

