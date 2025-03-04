using System.Threading.Tasks;
using IncliSafe.Shared.Models.DTOs;

namespace IncliSafeApi.Services.Interfaces
{
    public interface IMaintenancePredictionService
    {
        Task<MaintenancePredictionDTO> PredictMaintenanceAsync(int vehicleId);
    }
} 