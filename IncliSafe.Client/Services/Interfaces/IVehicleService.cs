using System.Threading.Tasks;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafe.Client.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<List<Vehiculo>> GetVehiculosAsync();
        Task<Vehiculo> GetVehiculoAsync(int id);
        Task<Vehiculo> CreateVehiculoAsync(Vehiculo vehiculo);
        Task<Vehiculo> UpdateVehiculoAsync(Vehiculo vehiculo);
        Task DeleteVehiculoAsync(int id);
        Task<List<Inspeccion>> GetInspeccionesAsync(int vehiculoId);
    }
} 