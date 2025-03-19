using System;
using System.Collections.Generic;
using System.Linq;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Shared.Models.Analysis.Core;
using IncliSafe.Shared.Models.Enums;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafe.Shared.Models.DTOs
{
    public static class IdConverter
    {
        public static int GuidToInt(Guid guid)
        {
            return Math.Abs(guid.GetHashCode());
        }

        public static Guid IntToGuid(int id)
        {
            return new Guid($"{id:D32}");
        }
    }

    public class AnalysisDTO
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required IncliSafe.Shared.Models.Enums.AnalysisType Type { get; set; }
        public required decimal Score { get; set; }
        public required DateTime AnalyzedAt { get; set; }
        public required Guid VehicleId { get; set; }
        public List<AnalysisDataDTO> DataPoints { get; set; } = new();
        public List<string> Recommendations { get; set; } = new();
        public string? Notes { get; set; }
        public required IncliSafe.Shared.Models.Enums.AlertSeverity Severity { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = new();

        public virtual IncliSafe.Shared.Models.Analysis.Core.AnalysisResult ToEntity()
        {
            return new IncliSafe.Shared.Models.Analysis.Core.AnalysisResult
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Type = Type,
                Score = Score,
                AnalyzedAt = AnalyzedAt,
                VehicleId = VehicleId,
                DataPoints = DataPoints.Select(d => d.ToEntity()).ToList(),
                Recommendations = Recommendations,
                Notes = Notes,
                Severity = Severity,
                Parameters = Parameters
            };
        }

        public virtual IncliSafe.Shared.Models.Analysis.Core.AnalysisResult FromEntity(IncliSafe.Shared.Models.Analysis.Core.AnalysisResult entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Description = entity.Description;
            Type = entity.Type;
            Score = entity.Score;
            AnalyzedAt = entity.AnalyzedAt;
            VehicleId = entity.VehicleId;
            DataPoints = entity.DataPoints.Select(d => AnalysisDataDTO.FromEntity(d)).ToList();
            Recommendations = entity.Recommendations;
            Notes = entity.Notes;
            Severity = entity.Severity;
            Parameters = entity.Parameters;
            return entity;
        }
    }

    public class AnalysisResultDTO : AnalysisDTO
    {
        public required decimal StabilityScore { get; set; }
        public required decimal SafetyScore { get; set; }
        public required decimal MaintenanceScore { get; set; }

        public override IncliSafe.Shared.Models.Analysis.Core.AnalysisResult ToEntity()
        {
            var result = base.ToEntity();
            result.Score = StabilityScore;
            return result;
        }

        public override IncliSafe.Shared.Models.Analysis.Core.AnalysisResult FromEntity(IncliSafe.Shared.Models.Analysis.Core.AnalysisResult entity)
        {
            base.FromEntity(entity);
            StabilityScore = entity.Score;
            SafetyScore = entity.Score;
            MaintenanceScore = entity.Score;
            return entity;
        }
    }

    public class AnalysisDataDTO
    {
        public required DateTime Timestamp { get; set; }
        public required decimal Value { get; set; }
        public required int SensorId { get; set; }
        public string? MetricType { get; set; }
        public Dictionary<string, decimal> AdditionalMetrics { get; set; } = new();

        public static AnalysisDataDTO FromEntity(IncliSafe.Shared.Models.Analysis.Core.AnalysisData entity)
        {
            return new AnalysisDataDTO
            {
                Timestamp = entity.Timestamp,
                Value = entity.Value,
                SensorId = entity.SensorId,
                MetricType = entity.MetricType,
                AdditionalMetrics = entity.AdditionalMetrics
            };
        }

        public IncliSafe.Shared.Models.Analysis.Core.AnalysisData ToEntity()
        {
            return new IncliSafe.Shared.Models.Analysis.Core.AnalysisData
            {
                Timestamp = Timestamp,
                Value = Value,
                SensorId = SensorId,
                MetricType = MetricType,
                AdditionalMetrics = AdditionalMetrics
            };
        }
    }

    public class AnomalyDTO : BaseDTO
    {
        public override int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required IncliSafe.Shared.Models.Enums.AnomalyType Type { get; set; }
        public required decimal Score { get; set; }
        public required DateTime DetectedAt { get; set; }
        public required IncliSafe.Shared.Models.Enums.AlertSeverity Severity { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = new();
        public override DateTime CreatedAt { get; set; }

        public static AnomalyDTO FromAnomaly(IncliSafe.Shared.Models.Analysis.Core.Anomaly anomaly)
        {
            return new AnomalyDTO
            {
                Id = Math.Abs(anomaly.Id.GetHashCode()),
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
        public override int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required IncliSafe.Shared.Models.Enums.PredictionType Type { get; set; }
        public required decimal Confidence { get; set; }
        public required DateTime PredictedAt { get; set; }
        public required DateTime ValidUntil { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = new();
        public override DateTime CreatedAt { get; set; }

        public static PredictionDTO FromPrediction(IncliSafe.Shared.Models.Analysis.Core.AnalysisPrediction prediction)
        {
            return new PredictionDTO
            {
                Id = Math.Abs(prediction.Id.GetHashCode()),
                Name = prediction.Name,
                Description = prediction.Description,
                Type = prediction.Type,
                Confidence = prediction.Probability,
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

        public static DetectedPatternDTO FromEntity(IncliSafe.Shared.Models.Analysis.Core.DetectedPattern entity)
        {
            return new DetectedPatternDTO
            {
                Type = entity.Type,
                Description = entity.Description,
                Confidence = entity.Confidence,
                DataPoints = entity.DataPoints.Select(d => PatternDataPointDTO.FromEntity(d)).ToList(),
                Metadata = entity.Metadata.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString() ?? "")
            };
        }
    }

    public class PatternDataPointDTO
    {
        public required DateTime Timestamp { get; set; }
        public string? Label { get; set; }
        public required decimal Value { get; set; }
        public Dictionary<string, decimal> Metrics { get; set; } = new();

        public static PatternDataPointDTO FromEntity(IncliSafe.Shared.Models.Analysis.Core.PatternDataPoint entity)
        {
            return new PatternDataPointDTO
            {
                Timestamp = entity.Timestamp,
                Label = entity.Label,
                Value = entity.Value,
                Metrics = entity.Metrics
            };
        }

        public IncliSafe.Shared.Models.Analysis.Core.PatternDataPoint ToEntity()
        {
            return new IncliSafe.Shared.Models.Analysis.Core.PatternDataPoint
            {
                Timestamp = Timestamp,
                Label = Label,
                Value = Value,
                Metrics = Metrics
            };
        }
    }

    public class TrendAnalysisDTO
    {
        public required int Id { get; set; }
        public required int VehicleId { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public required decimal TrendValue { get; set; }
        public required decimal Seasonality { get; set; }
        public required decimal Correlation { get; set; }
        public required IncliSafe.Shared.Models.Enums.TrendDirection Direction { get; set; }
        public required IncliSafe.Shared.Models.Enums.PerformanceTrend Performance { get; set; }
        public List<TrendDataDTO> Data { get; set; } = new();
        public Dictionary<string, object> Parameters { get; set; } = new();
        public Dictionary<string, object> Metadata { get; set; } = new();

        public static TrendAnalysisDTO FromEntity(IncliSafe.Shared.Models.Analysis.TrendAnalysis entity)
        {
            return new TrendAnalysisDTO
            {
                Id = entity.Id,
                VehicleId = entity.VehicleId,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                TrendValue = entity.TrendValue,
                Seasonality = entity.Seasonality,
                Correlation = entity.Correlation,
                Direction = entity.Direction,
                Performance = entity.Performance,
                Data = entity.Data.Select(d => TrendDataDTO.FromEntity(d)).ToList(),
                Parameters = entity.Parameters,
                Metadata = entity.Metadata
            };
        }

        public IncliSafe.Shared.Models.Analysis.TrendAnalysis ToEntity()
        {
            return new IncliSafe.Shared.Models.Analysis.TrendAnalysis
            {
                Id = Id,
                VehicleId = VehicleId,
                StartDate = StartDate,
                EndDate = EndDate,
                TrendValue = TrendValue,
                Seasonality = Seasonality,
                Correlation = Correlation,
                Direction = Direction,
                Performance = Performance,
                Data = Data.Select(d => d.ToEntity()).ToList(),
                Parameters = Parameters,
                Metadata = Metadata
            };
        }
    }

    public class TrendDataDTO
    {
        public required DateTime Timestamp { get; set; }
        public required decimal Value { get; set; }
        public string? Label { get; set; }
        public Dictionary<string, decimal> Metrics { get; set; } = new();

        public static TrendDataDTO FromEntity(IncliSafe.Shared.Models.Analysis.TrendData entity)
        {
            return new TrendDataDTO
            {
                Timestamp = entity.Timestamp,
                Value = entity.Value,
                Label = entity.Label,
                Metrics = entity.Metrics
            };
        }

        public IncliSafe.Shared.Models.Analysis.TrendData ToEntity()
        {
            return new IncliSafe.Shared.Models.Analysis.TrendData
            {
                Timestamp = Timestamp,
                Value = Value,
                Label = Label,
                Metrics = Metrics
            };
        }
    }
} 




