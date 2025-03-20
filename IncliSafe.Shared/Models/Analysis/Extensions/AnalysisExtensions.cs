using System;
using System.Collections.Generic;
using System.Linq;
using IncliSafe.Shared.Models.Analysis.Core;
using IncliSafe.Shared.Models.Common;
using IncliSafe.Shared.Models.DTOs;
using IncliSafe.Shared.Models.Enums;

namespace IncliSafe.Shared.Models.Analysis.Extensions;

public static class AnalysisExtensions
{
    public static AnalysisResult ToEntity(this AnalysisDTO dto)
    {
        var dataPoints = dto.DataPoints?.Select(dp => dp.ToEntity()).ToList() ?? new List<AnalysisData>();
        
        return new AnalysisResult
        {
            Id = dto.Id,
            CreatedAt = dto.CreatedAt,
            VehicleId = dto.VehicleId,
            Name = dto.Name,
            Description = dto.Description,
            Type = dto.AnalysisType,
            Score = dto.Score,
            AnalyzedAt = dto.AnalysisTime,
            AnalysisDate = dto.AnalysisTime,
            StabilityScore = dto.StabilityScore,
            SafetyScore = dto.SafetyScore,
            MaintenanceScore = dto.MaintenanceScore,
            EfficiencyScore = dto.EfficiencyScore,
            Notes = dto.Notes,
            DataPoints = dataPoints,
            Recommendations = dto.Recommendations ?? new List<string>(),
            Severity = dto.Severity,
            Parameters = dto.Parameters ?? new Dictionary<string, object>(),
            Confidence = 0.0m,
            Data = new AnalysisData
            {
                Timestamp = dto.AnalysisTime,
                Value = dto.Score,
                SensorId = 0,
                MetricType = "Overall",
                AdditionalMetrics = new Dictionary<string, decimal>
                {
                    { "Stability", dto.StabilityScore },
                    { "Safety", dto.SafetyScore },
                    { "Maintenance", dto.MaintenanceScore },
                    { "Efficiency", dto.EfficiencyScore }
                }
            }
        };
    }

    public static AnalysisDTO ToDTO(this AnalysisResult entity)
    {
        return new AnalysisDTO
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            VehicleId = entity.VehicleId,
            Name = entity.Name,
            Description = entity.Description,
            AnalysisType = entity.Type,
            Score = entity.Score,
            AnalysisTime = entity.AnalyzedAt,
            DobackAnalysisId = entity.Id,
            StabilityScore = entity.StabilityScore,
            SafetyScore = entity.SafetyScore,
            MaintenanceScore = entity.MaintenanceScore,
            EfficiencyScore = entity.EfficiencyScore,
            Notes = entity.Notes,
            DataPoints = entity.DataPoints.Select(dp => dp.ToDTO()).ToList(),
            Recommendations = entity.Recommendations,
            Severity = entity.Severity,
            Parameters = entity.Parameters
        };
    }

    public static AnalysisData ToEntity(this AnalysisDataDTO dto)
    {
        return new AnalysisData
        {
            Timestamp = dto.Timestamp,
            Value = dto.Value,
            SensorId = dto.SensorId,
            MetricType = dto.MetricType,
            AdditionalMetrics = dto.AdditionalMetrics ?? new Dictionary<string, decimal>()
        };
    }

    public static AnalysisDataDTO ToDTO(this AnalysisData entity)
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

    public static Anomaly ToEntity(this AnomalyDTO dto)
    {
        return new Anomaly
        {
            Id = dto.Id,
            CreatedAt = dto.CreatedAt,
            VehicleId = dto.VehicleId,
            DetectedAt = dto.DetectedAt,
            Type = dto.Type,
            Severity = dto.Severity,
            Name = dto.Name,
            Description = dto.Description,
            Score = dto.Score,
            ExpectedValue = dto.ExpectedValue,
            ActualValue = dto.ActualValue,
            Deviation = dto.Deviation,
            Parameters = dto.Parameters ?? new Dictionary<string, object>(),
            AnalysisId = dto.AnalysisId
        };
    }

    public static AnomalyDTO ToDTO(this Anomaly entity)
    {
        return new AnomalyDTO
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            VehicleId = entity.VehicleId,
            DetectedAt = entity.DetectedAt,
            Type = entity.Type,
            Severity = entity.Severity,
            Name = entity.Name,
            Description = entity.Description,
            Score = entity.Score,
            ExpectedValue = entity.ExpectedValue,
            ActualValue = entity.ActualValue,
            Deviation = entity.Deviation,
            Parameters = entity.Parameters,
            AnalysisId = entity.AnalysisId
        };
    }

    public static AnalysisPrediction ToEntity(this AnalysisPredictionDTO dto)
    {
        return new AnalysisPrediction
        {
            Id = dto.Id,
            CreatedAt = dto.CreatedAt,
            VehicleId = dto.VehicleId,
            PredictedAt = dto.PredictedAt,
            Type = dto.Type,
            Probability = dto.Probability,
            Name = dto.Name,
            Description = dto.Description,
            PredictedValue = dto.PredictedValue,
            ValidUntil = dto.ValidUntil,
            Parameters = dto.Parameters ?? new Dictionary<string, object>(),
            AnalysisId = dto.AnalysisId
        };
    }

    public static AnalysisPredictionDTO ToDTO(this AnalysisPrediction entity)
    {
        return new AnalysisPredictionDTO
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            VehicleId = entity.VehicleId,
            PredictedAt = entity.PredictedAt,
            Type = entity.Type,
            Probability = entity.Probability,
            Name = entity.Name,
            Description = entity.Description,
            PredictedValue = entity.PredictedValue,
            ValidUntil = entity.ValidUntil,
            Parameters = entity.Parameters,
            AnalysisId = entity.AnalysisId
        };
    }
} 