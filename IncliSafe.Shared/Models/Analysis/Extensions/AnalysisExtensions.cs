using IncliSafe.Shared.Models.Base;
using IncliSafe.Shared.Models.Enums;
using IncliSafe.Shared.Models.Analysis.Core;
using IncliSafe.Shared.Models.Analysis.DTOs;

namespace IncliSafe.Shared.Models.Analysis.Extensions;

public static class AnalysisExtensions
{
    public static AnalysisPrediction ToEntity(this AnalysisPredictionDTO dto)
    {
        if (dto == null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        return new AnalysisPrediction
        {
            VehicleId = dto.VehicleId,
            GeneratedAt = dto.GeneratedAt,
            Type = dto.Type,
            Description = dto.Description,
            Score = dto.Score,
            Confidence = dto.Confidence,
            PredictedValue = dto.PredictedValue,
            ValidUntil = dto.ValidUntil,
            IsActive = dto.IsActive,
            IsValidated = dto.IsValidated,
            Parameters = dto.Parameters,
            TrainingData = new(),
            Alerts = new(),
            Notifications = new(),
            ModelVersion = dto.ModelVersion,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt
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
            VehicleId = entity.VehicleId,
            GeneratedAt = entity.GeneratedAt,
            Type = entity.Type,
            Description = entity.Description,
            Score = entity.Score,
            Confidence = entity.Confidence,
            PredictedValue = entity.PredictedValue,
            ValidUntil = entity.ValidUntil,
            IsActive = entity.IsActive,
            IsValidated = entity.IsValidated,
            Parameters = entity.Parameters,
            ModelVersion = entity.ModelVersion,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt ?? DateTime.UtcNow,
            Resolution = entity.Description
        };
    }
} 