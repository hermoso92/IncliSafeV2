using System;
using System.Collections.Generic;
using IncliSafe.Shared.Models.DTOs;

namespace IncliSafe.Shared.Models.Analysis.Data;

public static class DataPointExtensions
{
    public static DataPoint ToEntity(this DataPointDTO dto)
    {
        if (dto == null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        return new DataPoint
        {
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
            CreatedAt = DateTime.UtcNow,
            Name = entity.Name,
            Unit = entity.Unit,
            Timestamp = entity.Timestamp,
            Label = entity.Label,
            Value = entity.Value,
            Metadata = entity.Metadata,
            Metrics = entity.Metrics
        };
    }
} 