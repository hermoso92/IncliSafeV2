using System;
using System.Collections.Generic;
using System.Linq;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Shared.Models.Enums;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafe.Shared.Models.DTOs
{
    public class AnalysisDTO : BaseDTO
    {
        public override Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public required Enums.AnalysisType Type { get; set; }
        public virtual decimal StabilityScore { get; set; }
        public virtual Guid VehicleId { get; set; }
        public List<string> Recommendations { get; set; } = new();
        public string? Notes { get; set; }
        public override DateTime CreatedAt { get; set; }
    }

    public class DobackAnalysisDTO : AnalysisDTO
    {
        public override string Name { get; set; }
        public override string Description { get; set; }
        public override decimal StabilityScore { get; set; }
        public override Guid VehicleId { get; set; }
        public List<DobackDataPoint> DataPoints { get; set; } = new();
        public AlertSeverity Severity { get; set; }

        public static DobackAnalysisDTO FromDobackAnalysis(DobackAnalysis analysis)
        {
            return new DobackAnalysisDTO
            {
                Id = analysis.Id,
                Name = analysis.Name,
                Description = analysis.Description,
                Type = analysis.Type,
                StabilityScore = analysis.StabilityScore,
                VehicleId = analysis.VehicleId,
                DataPoints = analysis.DataPoints,
                Recommendations = analysis.Recommendations,
                Notes = analysis.Notes,
                CreatedAt = analysis.CreatedAt,
                Severity = analysis.Severity
            };
        }
    }

    public class DobackDataDTO
    {
        public required DateTime Timestamp { get; set; }
        public required decimal Value { get; set; }
        public required int SensorId { get; set; }
        public string? MetricType { get; set; }
        public Dictionary<string, decimal> AdditionalMetrics { get; set; } = new();

        public static DobackDataDTO FromEntity(DobackData entity)
        {
            return new DobackDataDTO
            {
                Timestamp = entity.Timestamp,
                Value = entity.Value,
                SensorId = entity.SensorId,
                MetricType = entity.MetricType,
                AdditionalMetrics = entity.AdditionalMetrics
            };
        }
    }

    public class AnomalyDTO : BaseDTO
    {
        public override Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required AnomalyType Type { get; set; }
        public required decimal Score { get; set; }
        public required DateTime DetectedAt { get; set; }
        public required AlertSeverity Severity { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = new();
        public override DateTime CreatedAt { get; set; }

        public static AnomalyDTO FromAnomaly(Anomaly anomaly)
        {
            return new AnomalyDTO
            {
                Id = anomaly.Id,
                Name = anomaly.Name,
                Description = anomaly.Description,
                Type = anomaly.Type,
                Score = anomaly.Score,
                DetectedAt = anomaly.DetectedAt,
                Severity = anomaly.Severity,
                Parameters = anomaly.Parameters,
                CreatedAt = anomaly.CreatedAt
            };
        }
    }

    public class PredictionDTO : BaseDTO
    {
        public override Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required PredictionType Type { get; set; }
        public required decimal Confidence { get; set; }
        public required DateTime PredictedAt { get; set; }
        public required DateTime ValidUntil { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = new();
        public override DateTime CreatedAt { get; set; }

        public static PredictionDTO FromPrediction(Prediction prediction)
        {
            return new PredictionDTO
            {
                Id = prediction.Id,
                Name = prediction.Name,
                Description = prediction.Description,
                Type = prediction.Type,
                Confidence = prediction.Confidence,
                PredictedAt = prediction.PredictedAt,
                ValidUntil = prediction.ValidUntil,
                Parameters = prediction.Parameters,
                CreatedAt = prediction.CreatedAt
            };
        }
    }

    public class DetectedPatternDTO
    {
        public required string Type { get; set; }
        public string? Description { get; set; }
        public required decimal Confidence { get; set; }
        public List<PatternDataPointDTO> DataPoints { get; set; } = new();
        public Dictionary<string, string> Metadata { get; set; } = new();

        public static DetectedPatternDTO FromEntity(Patterns.DetectedPattern entity)
        {
            var dto = new DetectedPatternDTO
            {
                Type = entity.PatternType.ToString(),
                Description = entity.Description ?? string.Empty,
                Confidence = entity.Confidence,
                Metadata = entity.Metadata
            };

            dto.DataPoints = entity.DataPoints.Select(p => PatternDataPointDTO.FromEntity(p)).ToList();
            return dto;
        }
    }

    public class PatternDataPointDTO
    {
        public required DateTime Timestamp { get; set; }
        public string? Label { get; set; }
        public required decimal Value { get; set; }
        public Dictionary<string, decimal> Metrics { get; set; } = new();

        public static PatternDataPointDTO FromEntity(Patterns.PatternDataPoint entity)
        {
            return new PatternDataPointDTO
            {
                Timestamp = entity.Timestamp,
                Label = entity.Label,
                Value = entity.Value,
                Metrics = entity.AdditionalMetrics ?? new Dictionary<string, decimal>()
            };
        }
    }

    public class TrendAnalysisDTO : AnalysisDTO
    {
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public required decimal TrendValue { get; set; }
        public required decimal Seasonality { get; set; }
        public required decimal Correlation { get; set; }
        public List<TrendDataDTO> Data { get; set; } = new();

        public static TrendAnalysisDTO FromEntity(TrendAnalysis entity)
        {
            var dto = new TrendAnalysisDTO
            {
                Id = entity.Id,
                VehicleId = entity.VehicleId,
                AnalysisTime = entity.AnalysisDate,
                Type = entity.Type,
                StabilityScore = entity.StabilityScore,
                SafetyScore = entity.SafetyScore,
                MaintenanceScore = entity.MaintenanceScore,
                Notes = entity.Notes,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                TrendValue = entity.TrendValue,
                Seasonality = entity.Seasonality,
                Correlation = entity.Correlation
            };

            dto.Data = entity.Data.Select(TrendDataDTO.FromEntity).ToList();
            return dto;
        }
    }

    public class TrendDataDTO
    {
        public required DateTime Timestamp { get; set; }
        public required decimal Value { get; set; }
        public string? Label { get; set; }
        public Dictionary<string, decimal> Metrics { get; set; } = new();

        public static TrendDataDTO FromEntity(TrendData entity)
        {
            return new TrendDataDTO
            {
                Timestamp = entity.Timestamp,
                Value = entity.Value,
                Label = entity.Label,
                Metrics = entity.Metrics
            };
        }
    }
} 




