using System;

namespace IncliSafe.Shared.Models.Analysis
{
    public class DobackFileInfo
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public int VehicleId { get; set; }
    }
} 