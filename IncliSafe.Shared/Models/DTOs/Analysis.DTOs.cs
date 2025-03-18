using IncliSafe.Shared.Models.Enums;
using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafe.Shared.Models.DTOs
{
    public class AnalysisDTO
    {
        public required int Id { get; set; }
        public required int VehicleId { get; set; }
        public required DateTime AnalysisDate { get; set; }
        public required AnalysisType Type { get; set; }
        public required decimal StabilityScore { get; set; }
        public required decimal SafetyScore { get; set; }
        public required decimal MaintenanceScore { get; set; }
        public string? Notes { get; set; }
        public List<AnomalyDTO> Anomalies { get; set; } = new();
        public List<PredictionDTO> Predictions { get; set; } = new();

        public static AnalysisDTO FromEntity(AnalysisBase entity)
        {
            return new AnalysisDTO
            {
                Id = entity.Id,
                VehicleId = entity.VehicleId,
                AnalysisDate = entity.AnalysisDate,
                Type = entity.Type,
                StabilityScore = entity.StabilityScore,
                SafetyScore = entity.SafetyScore,
                MaintenanceScore = entity.MaintenanceScore,
                Notes = entity.Notes
            };
        }
    }

    public class DobackAnalysisDTO : AnalysisDTO
    {
        public required int DobackId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal StabilityIndex { get; set; }
        public required decimal SafetyIndex { get; set; }
        public required decimal MaintenanceIndex { get; set; }
        public List<DobackDataDTO> Data { get; set; } = new();
        public List<DetectedPatternDTO> Patterns { get; set; } = new();

        public static DobackAnalysisDTO FromEntity(DobackAnalysis entity)
        {
            var dto = new DobackAnalysisDTO
            {
                Id = entity.Id,
                VehicleId = entity.VehicleId,
                AnalysisDate = entity.AnalysisDate,
                Type = entity.Type,
                StabilityScore = entity.StabilityScore,
                SafetyScore = entity.SafetyScore,
                MaintenanceScore = entity.MaintenanceScore,
                Notes = entity.Notes,
                DobackId = entity.DobackId,
                Name = entity.Name,
                Description = entity.Description,
                StabilityIndex = entity.StabilityIndex,
                SafetyIndex = entity.SafetyIndex,
                MaintenanceIndex = entity.MaintenanceIndex
            };

            dto.Data = entity.Data.Select(d => DobackDataDTO.FromEntity(d)).ToList();
            dto.Anomalies = entity.Anomalies.Select(a => AnomalyDTO.FromEntity(a)).ToList();
            dto.Predictions = entity.Predictions.Select(p => PredictionDTO.FromEntity(p)).ToList();
            dto.Patterns = entity.Patterns.Select(p => DetectedPatternDTO.FromEntity(p)).ToList();

            return dto;
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

    public class AnomalyDTO
    {
        public required int Id { get; set; }
        public required int VehicleId { get; set; }
        public required DateTime DetectedAt { get; set; }
        public required AnomalyType Type { get; set; }
        public required AlertSeverity Severity { get; set; }
        public required string Description { get; set; } = string.Empty;
        public required decimal ExpectedValue { get; set; }
        public required decimal ActualValue { get; set; }
        public required decimal Deviation { get; set; }
        public int? AnalysisId { get; set; }

        public static AnomalyDTO FromEntity(Anomaly entity)
        {
            return new AnomalyDTO
            {
                Id = entity.Id,
                VehicleId = entity.VehicleId,
                DetectedAt = entity.DetectedAt,
                Type = entity.Type,
                Severity = entity.Severity,
                Description = entity.Description,
                ExpectedValue = entity.ExpectedValue,
                ActualValue = entity.ActualValue,
                Deviation = entity.Deviation,
                AnalysisId = entity.AnalysisId
            };
        }
    }

    public class PredictionDTO
    {
        public required int Id { get; set; }
        public required int VehicleId { get; set; }
        public required DateTime PredictedAt { get; set; }
        public required PredictionType Type { get; set; }
        public required decimal Probability { get; set; }
        public required string Description { get; set; } = string.Empty;
        public required decimal PredictedValue { get; set; }
        public int? AnalysisId { get; set; }

        public static PredictionDTO FromEntity(AnalysisPrediction entity)
        {
            return new PredictionDTO
            {
                Id = entity.Id,
                VehicleId = entity.VehicleId,
                PredictedAt = entity.PredictedAt,
                Type = entity.Type,
                Probability = entity.Probability,
                Description = entity.Description,
                PredictedValue = entity.PredictedValue,
                AnalysisId = entity.AnalysisId
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
                AnalysisDate = entity.AnalysisDate,
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




