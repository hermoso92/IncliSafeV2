using IncliSafe.Shared.Models.Enums;
using System;
using System.Collections.Generic;

namespace IncliSafe.Shared.Models.DTOs
{
    public class TrendAnalysisDTO
    {
        public int VehicleId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<TrendMetricDTO> Metrics { get; set; }
        public List<PatternDTO> Patterns { get; set; }
        public List<PredictionDTO> Predictions { get; set; }
        public string Summary { get; set; }
        public decimal OverallTrend { get; set; }
        public TrendDirection Direction { get; set; }
        public DateTime GeneratedAt { get; set; }
    }

    public class TrendMetricDTO
    {
        public TrendMetric Type { get; set; }
        public decimal Value { get; set; }
        public decimal Change { get; set; }
        public TrendDirection Direction { get; set; }
        public string Description { get; set; }
    }

    public class PatternDTO
    {
        public PatternType Type { get; set; }
        public string Description { get; set; }
        public decimal Confidence { get; set; }
        public DateTime DetectedAt { get; set; }
        public string Impact { get; set; }
    }

    public class PredictionDTO
    {
        public PredictionType Type { get; set; }
        public decimal Value { get; set; }
        public decimal Confidence { get; set; }
        public DateTime PredictedAt { get; set; }
        public string Description { get; set; }
    }
} 