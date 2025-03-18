using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Notifications;
using IncliSafe.Shared.Models.Analysis.Core;

namespace IncliSafe.Shared.Models.Analysis
{
    public class DobackAnalysis : AnalysisBase
    {
        public required int DobackId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal StabilityIndex { get; set; }
        public required decimal SafetyIndex { get; set; }
        public required decimal MaintenanceIndex { get; set; }
        public List<DobackData> Data { get; set; } = new();
        public List<Anomaly> Anomalies { get; set; } = new();
        public List<AnalysisPrediction> Predictions { get; set; } = new();
        public List<DetectedPattern> Patterns { get; set; } = new();
    }

    public class DobackData
    {
        public required DateTime Timestamp { get; set; }
        public required decimal Value { get; set; }
        public required int SensorId { get; set; }
        public string? MetricType { get; set; }
        public Dictionary<string, decimal> AdditionalMetrics { get; set; } = new();
    }

    public class DetectedPattern
    {
        public required string Type { get; set; }
        public string? Description { get; set; }
        public required decimal Confidence { get; set; }
        public List<PatternDataPoint> DataPoints { get; set; } = new();
        public Dictionary<string, string> Metadata { get; set; } = new();
    }

    public class PatternDataPoint
    {
        public required DateTime Timestamp { get; set; }
        public string? Label { get; set; }
        public required decimal Value { get; set; }
        public Dictionary<string, decimal> Metrics { get; set; } = new();
    }
} 