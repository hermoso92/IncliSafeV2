using System.Threading.Tasks;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafeApi.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<List<Vehiculo>> GetVehiculosAsync();
        Task<Vehiculo?> GetVehicleAsync(int id);
        Task<Vehiculo> CreateVehicleAsync(Vehiculo vehiculo);
        Task<bool> UpdateVehicleAsync(Vehiculo vehiculo);
        Task<bool> DeleteVehicleAsync(int id);
        Task<List<License>> GetVehicleLicensesAsync(int vehicleId);
    }
} 