using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis
{
    public class TrendInfo
    {
        public required DateTime StartTime { get; set; }
        public required DateTime EndTime { get; set; }
        public required decimal StartValue { get; set; }
        public required decimal EndValue { get; set; }
        public required decimal ChangeRate { get; set; }
        public required decimal Confidence { get; set; }
        public required TrendType Type { get; set; }
        public Dictionary<string, object> Metadata { get; set; } = new();
    }
} 

