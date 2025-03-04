using IncliSafe.Shared.Models.DTOs;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafeApi.Services.Mapping
{
    public static class LicenseMapper
    {
        public static LicenseDTO ToDTO(this License license)
        {
            return new LicenseDTO
            {
                Id = license.Id,
                Type = license.Type,
                ExpirationDate = license.ExpirationDate,
                IsActive = license.IsActive
            };
        }

        public static License ToEntity(this LicenseDTO dto)
        {
            return new License
            {
                Id = dto.Id,
                Type = dto.Type,
                ExpirationDate = dto.ExpirationDate,
                IsActive = dto.IsActive
            };
        }
    }
} 