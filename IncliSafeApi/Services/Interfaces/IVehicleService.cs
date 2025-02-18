using System.Threading.Tasks;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Entities;
using IncliSafe.Shared.Models.DTOs;

namespace IncliSafe.Api.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<List<VehiculoDTO>> GetVehiclesAsync(int ownerId);
        Task<VehiculoDTO> GetVehicleAsync(int id);
        Task<VehiculoDTO> CreateVehicleAsync(VehiculoDTO dto);
        Task<VehiculoDTO> UpdateVehicleAsync(int id, VehiculoDTO dto);
        Task<bool> DeleteVehicleAsync(int id);
        Task<bool> ValidateLicenseAsync(int vehicleId);
        Task<List<InspeccionDTO>> GetInspeccionesAsync(int vehicleId);
        Task<bool> AddInspeccionAsync(int vehicleId, InspeccionDTO inspeccion);
        Task<bool> ExistsAsync(int id);
        Task<LicenseDTO?> GetLicenseAsync(int vehicleId);
        Task<bool> UpdateLicenseAsync(int vehicleId, LicenseDTO license);
        Task<LicenseDTO> CreateLicenseAsync(int vehicleId, LicenseType type);
        Task<VehicleStatsDTO> GetVehicleStatsAsync(int vehicleId);
        Task<List<MonthlyStatsDTO>> GetMonthlyStatsAsync(int vehicleId, DateTime startDate, DateTime endDate);
        Task<VehicleSummaryDTO> GetVehicleSummaryAsync(int vehicleId);
        Task<List<VehicleAlertDTO>> GetVehicleAlertsAsync(int vehicleId);
        Task<bool> MarkAlertAsReadAsync(int vehicleId, int alertId);
        Task<VehicleAlertDTO> CreateAlertAsync(int vehicleId, VehicleAlertDTO alert);
    }
} 