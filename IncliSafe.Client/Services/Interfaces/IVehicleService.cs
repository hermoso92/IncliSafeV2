using System.Threading.Tasks;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafe.Client.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<List<Vehiculo>> GetVehiculos();
        Task<Vehiculo?> GetVehicle(int id);
        Task<List<Vehiculo>> GetUserVehiculos(int userId);
        Task<Vehiculo?> CreateVehiculo(Vehiculo vehiculo);
        Task<bool> UpdateVehiculo(Vehiculo vehiculo);
        Task<bool> DeleteVehiculo(int id);
        Task<List<Inspeccion>> GetInspeccionesAsync(int vehiculoId);
    }
} 