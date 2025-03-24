using System;
using System.Collections.Generic;
using System.Linq;
using IncliSafe.Shared.Models.Analysis.Core;
using IncliSafe.Shared.Models.Analysis.Data;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.DTOs;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis.Extensions;

public static class AnalysisExtensions
{
    public static Core.AnalysisResult ToEntity(this AnalysisDTO dto)
    {
        if (dto == null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        var parameters = new Dictionary<string, object>();
        if (dto.Parameters != null)
        {
            foreach (var kvp in dto.Parameters)
            {
                parameters[kvp.Key] = kvp.Value;
            }
        }

        return new Core.AnalysisResult
        {
            Id = dto.Id,
            CreatedAt = dto.CreatedAt,
            VehicleId = int.Parse(dto.VehicleId),
            Name = dto.Name ?? string.Empty,
            Description = dto.Description,
            Type = dto.AnalysisType,
            Score = dto.Score,
            AnalyzedAt = dto.AnalyzedAt,
            AnalysisDate = dto.AnalysisDate,
            StabilityScore = dto.StabilityScore,
            SafetyScore = dto.SafetyScore,
            MaintenanceScore = dto.MaintenanceScore,
            EfficiencyScore = dto.EfficiencyScore,
            Notes = dto.Notes,
            DataPoints = dto.DataPoints?.Select(dp => dp.Value).ToList() ?? new List<decimal>(),
            Recommendations = dto.Recommendations ?? new List<string>(),
            Severity = dto.Severity,
            Parameters = parameters,
            Confidence = dto.Confidence,
            Data = System.Text.Json.JsonSerializer.Serialize(dto.Data)
        };
    }

    public static AnalysisDTO ToDTO(this Core.AnalysisResult entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        var parameters = new Dictionary<string, string>();
        foreach (var kvp in entity.Parameters)
        {
            parameters[kvp.Key] = kvp.Value?.ToString() ?? string.Empty;
        }

        var analysisData = System.Text.Json.JsonSerializer.Deserialize<AnalysisDataDTO>(entity.Data) 
            ?? new AnalysisDataDTO
            {
                Id = 0,
                CreatedAt = DateTime.UtcNow,
                Timestamp = DateTime.UtcNow,
                Value = 0,
                SensorId = 0,
                MetricType = string.Empty,
                AdditionalMetrics = new Dictionary<string, decimal>()
            };

        return new AnalysisDTO
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            VehicleId = entity.VehicleId.ToString(),
            Name = entity.Name,
            Description = entity.Description,
            Type = entity.Type.ToString(),
            Score = entity.Score,
            AnalyzedAt = entity.AnalyzedAt,
            AnalysisDate = entity.AnalysisDate,
            StabilityScore = entity.StabilityScore,
            SafetyScore = entity.SafetyScore,
            MaintenanceScore = entity.MaintenanceScore,
            EfficiencyScore = entity.EfficiencyScore,
            Notes = entity.Notes,
            DataPoints = entity.DataPoints?.Select(value => new DataPointDTO 
            { 
                Id = 0,
                CreatedAt = DateTime.UtcNow,
                Name = "DataPoint",
                Unit = "Unit",
                Timestamp = DateTime.UtcNow,
                Value = value,
                Metadata = new Dictionary<string, string>(),
                Metrics = new Dictionary<string, decimal>()
            }).ToList() ?? new List<DataPointDTO>(),
            Recommendations = entity.Recommendations,
            Severity = entity.Severity,
            Parameters = parameters,
            Confidence = entity.Confidence,
            Data = analysisData,
            AnalysisType = entity.Type,
            AnalysisTime = TimeSpan.FromMinutes(30),
            DobackAnalysisId = 1
        };
    }

    public static DataPoint ToEntity(this DataPointDTO dto)
    {
        if (dto == null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        return new DataPoint
        {
            Id = dto.Id,
            CreatedAt = dto.CreatedAt,
            Name = dto.Name,
            Unit = dto.Unit,
            Timestamp = dto.Timestamp,
            Label = dto.Label,
            Value = dto.Value,
            Metadata = dto.Metadata,
            Metrics = dto.Metrics
        };
    }

    public static DataPointDTO ToDTO(this DataPoint entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        return new DataPointDTO
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            Name = entity.Name,
            Unit = entity.Unit,
            Timestamp = entity.Timestamp,
            Label = entity.Label,
            Value = entity.Value,
            Metadata = entity.Metadata,
            Metrics = entity.Metrics
        };
    }

    public static AnalysisData ToEntity(this AnalysisDataDTO dto)
    {
        if (dto == null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        return new AnalysisData
        {
            Timestamp = dto.Timestamp,
            Value = dto.Value,
            SensorId = dto.SensorId,
            MetricType = dto.MetricType,
            AdditionalMetrics = dto.AdditionalMetrics
        };
    }

    public static AnalysisDataDTO ToDTO(this AnalysisData entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        return new AnalysisDataDTO
        {
            Id = 0,
            CreatedAt = DateTime.UtcNow,
            Timestamp = entity.Timestamp,
            Value = entity.Value,
            SensorId = entity.SensorId,
            MetricType = entity.MetricType,
            AdditionalMetrics = entity.AdditionalMetrics
        };
    }

    public static Anomaly ToEntity(this AnomalyDTO dto)
    {
        if (dto == null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        return new Anomaly
        {
            Id = dto.Id,
            CreatedAt = dto.CreatedAt,
            Name = dto.Name,
            Description = dto.Description ?? string.Empty,
            Score = (double)dto.Score,
            Severity = dto.Severity.ToString(),
            Type = dto.Type.ToString(),
            DetectedAt = dto.DetectedAt,
            ExpectedValue = dto.ExpectedValue,
            ActualValue = dto.ActualValue,
            Parameters = dto.Parameters.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value),
            Resolution = dto.Resolution,
            ModelVersion = dto.ModelVersion,
            IsActive = true
        };
    }

    public static AnomalyDTO ToDTO(this Anomaly entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        return new AnomalyDTO
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            Name = entity.Name,
            Description = entity.Description,
            Score = (decimal)entity.Score,
            Severity = Enum.Parse<AnalysisSeverity>(entity.Severity),
            Type = Enum.Parse<AnomalyType>(entity.Type),
            DetectedAt = entity.DetectedAt,
            ExpectedValue = Convert.ToDecimal(entity.ExpectedValue),
            ActualValue = Convert.ToDecimal(entity.ActualValue),
            Deviation = Math.Abs(Convert.ToDecimal(entity.ExpectedValue) - Convert.ToDecimal(entity.ActualValue)),
            Parameters = entity.Parameters.ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.ToString() ?? string.Empty),
            Resolution = entity.Resolution,
            ModelVersion = entity.ModelVersion
        };
    }

    public static AnalysisPrediction ToEntity(this AnalysisPredictionDTO dto)
    {
        if (dto == null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        return new AnalysisPrediction
        {
            Id = dto.Id,
            CreatedAt = dto.CreatedAt,
            Name = dto.Name,
            Description = dto.Description ?? string.Empty,
            Type = dto.Type.ToString(),
            PredictedAt = dto.PredictedAt,
            Probability = (double)dto.Probability,
            ValidUntil = dto.ValidUntil,
            Parameters = dto.Parameters.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value),
            Risk = dto.Risk,
            IsActive = dto.IsActive,
            UpdatedAt = dto.UpdatedAt,
            Resolution = dto.Resolution ?? string.Empty,
            ModelVersion = dto.ModelVersion ?? "1.0"
        };
    }

    public static AnalysisPredictionDTO ToDTO(this AnalysisPrediction entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        return new AnalysisPredictionDTO
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            Name = entity.Name,
            Description = entity.Description,
            Type = Enum.Parse<PredictionType>(entity.Type),
            PredictedAt = entity.PredictedAt,
            Probability = (decimal)entity.Probability,
            ValidUntil = entity.ValidUntil,
            Parameters = entity.Parameters.ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.ToString() ?? string.Empty),
            Risk = entity.Risk,
            IsActive = entity.IsActive,
            UpdatedAt = entity.UpdatedAt,
            Resolution = entity.Resolution,
            ModelVersion = entity.ModelVersion,
            PredictedValue = Convert.ToDecimal(entity.Parameters.GetValueOrDefault("PredictedValue", 0))
        };
    }
} 