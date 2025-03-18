using System.Threading.Tasks;
using IncliSafe.Shared.Models.DTOs;

namespace IncliSafeApi.Services.Interfaces
{
    public interface IVehicleMetricsService
    {
        Task<VehicleMetricsDTO> GetVehicleMetricsAsync(int vehicleId);
        Task UpdateMetricsAsync(int vehicleId);
        Task<decimal> CalculateStabilityScoreAsync(int vehicleId);
    }
} 