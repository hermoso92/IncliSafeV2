using System.Threading.Tasks;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafeApi.Services.Interfaces
{
    public interface ILicenseService
    {
        Task<License?> GetLicenseAsync(int id);
        Task<License> CreateLicenseAsync(License license);
        Task<bool> UpdateLicenseAsync(License license);
        Task<bool> DeleteLicenseAsync(int id);
        Task<License?> GetVehicleLicenseAsync(int vehicleId);
        Task<List<License>> GetActiveLicensesAsync();
        Task<bool> ValidateLicenseAsync(string licenseKey);
    }
} 