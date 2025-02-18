using System.Threading.Tasks;
using IncliSafe.Shared.Models.DTOs;

namespace IncliSafeApi.Services.Interfaces
{
    public interface IMeterService
    {
        Task<VehicleMetricsDTO> GetVehicleMetricsAsync(int vehicleId);
        Task ProcessNewMeterDataAsync(int vehicleId, decimal value);
    }
} 