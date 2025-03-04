using System.Threading.Tasks;
using IncliSafe.Shared.Models.DTOs;
using IncliSafe.Shared.Models.Notifications;

namespace IncliSafeApi.Services.Interfaces
{
    public interface IAlertGenerationService
    {
        Task<VehicleAlertDTO> CreateAlertAsync(int vehicleId, VehicleAlertDTO alert);
        Task<bool> GenerateInspectionAlertAsync(int vehicleId, InspeccionDTO inspeccion);
        Task<bool> GenerateMaintenanceAlertAsync(int vehicleId, VehicleMaintenanceDTO maintenance);
        Task<bool> GenerateLicenseExpirationAlertAsync(int vehicleId);
        Task<bool> CheckThresholdsAsync(int vehicleId);
        Task ProcessAlertsAsync();
    }
} 