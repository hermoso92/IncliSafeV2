using IncliSafe.Shared.Models;

namespace IncliSafe.Client.Services.Interfaces
{
    public interface IInspectionService
    {
        Task<List<Inspeccion>> GetInspeccionesAsync();
        Task<Inspeccion> GetInspeccionAsync(int id);
        Task<Inspeccion> CreateInspeccionAsync(Inspeccion inspeccion);
        Task<Inspeccion> UpdateInspeccionAsync(Inspeccion inspeccion);
        Task DeleteInspeccionAsync(int id);
    }
} 