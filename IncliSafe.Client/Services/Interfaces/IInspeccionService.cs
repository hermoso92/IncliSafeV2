using System.Collections.Generic;
using System.Threading.Tasks;
using IncliSafe.Shared.Models.DTOs;

namespace IncliSafe.Client.Services.Interfaces
{
    public interface IInspeccionService
    {
        Task<List<InspeccionDTO>> GetInspeccionesAsync();
        Task<InspeccionDTO> GetInspeccionAsync(int id);
        Task<InspeccionDTO> CreateInspeccionAsync(InspeccionDTO inspeccion);
        Task<InspeccionDTO> UpdateInspeccionAsync(InspeccionDTO inspeccion);
        Task<bool> DeleteInspeccionAsync(int id);
    }
} 