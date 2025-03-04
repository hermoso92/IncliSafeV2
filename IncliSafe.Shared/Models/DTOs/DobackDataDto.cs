using System;

namespace IncliSafe.Shared.Models.DTOs
{
    public class DobackDataDto
    {
        public double AccelerationX { get; set; }
        public double AccelerationY { get; set; }
        public double AccelerationZ { get; set; }
        public double Roll { get; set; }
        public double Pitch { get; set; }
        public double Yaw { get; set; }
        public double Speed { get; set; }
        public double StabilityIndex { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double TimeAntWifi { get; set; }
        public DateTime Timestamp { get; set; }
    }
} 