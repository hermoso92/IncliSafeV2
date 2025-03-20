using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Notifications;
using IncliSafe.Shared.Models.Analysis.Core;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis
{
    public class AnalysisTypeInfo
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required AnalysisType Type { get; set; }
        public required decimal Confidence { get; set; }
        public required AlertSeverity Severity { get; set; }
        public required string Category { get; set; }
        public required string Source { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUsed { get; set; }
        public bool IsActive { get; set; }
        public List<string> Keywords { get; set; } = new();
        public List<string> Tags { get; set; } = new();
        public Dictionary<string, object> Parameters { get; set; } = new();
        public Dictionary<string, object> Metadata { get; set; } = new();
    }

    public class AnalysisTypeData
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required AnalysisType Type { get; set; }
        public required decimal Confidence { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public List<AnalysisDataPoint> DataPoints { get; set; } = new();
        public Dictionary<string, string> Metadata { get; set; } = new();
    }

    public class AnalysisDataPoint
    {
        public required DateTime Timestamp { get; set; }
        public string? Label { get; set; }
        public required decimal Value { get; set; }
        public Dictionary<string, decimal> Metrics { get; set; } = new();
    }
} 