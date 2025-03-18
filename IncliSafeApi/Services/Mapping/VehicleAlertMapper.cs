using IncliSafe.Shared.Models.DTOs;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.Notifications;

namespace IncliSafeApi.Services.Mapping
{
    public static class VehicleAlertMapper
    {
        public static VehicleAlertDTO ToDTO(this VehicleAlert alert)
        {
            return new VehicleAlertDTO
            {
                Id = alert.Id,
                VehicleId = alert.VehicleId,
                Title = alert.Title,
                Message = alert.Message,
                Type = alert.Type,
                Severity = alert.Severity,
                CreatedAt = alert.CreatedAt,
                IsRead = alert.IsRead,
                ReadAt = alert.ReadAt
            };
        }

        public static VehicleAlert ToEntity(this VehicleAlertDTO dto)
        {
            return new VehicleAlert
            {
                Id = dto.Id,
                VehicleId = dto.VehicleId,
                Title = dto.Title,
                Message = dto.Message,
                Type = dto.Type,
                Severity = dto.Severity,
                CreatedAt = dto.CreatedAt,
                IsRead = dto.IsRead,
                ReadAt = dto.ReadAt
            };
        }
    }
} 