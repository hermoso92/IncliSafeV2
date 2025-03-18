using IncliSafe.Shared.Models.Enums;
using System;

namespace IncliSafe.Shared.Models.Analysis
{
    public class DobackFileInfo
    {
        public required int Id { get; set; }
        public required string FileName { get; set; } = string.Empty;
        public required DateTime Timestamp { get; set; }
        public required int VehicleId { get; set; }
    }
} 

