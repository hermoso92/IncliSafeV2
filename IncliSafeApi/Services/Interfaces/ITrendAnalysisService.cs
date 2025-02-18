using System.Threading.Tasks;
using IncliSafe.Shared.Models.DTOs;

namespace IncliSafeApi.Services.Interfaces
{
    public interface ITrendAnalysisService
    {
        Task<TrendAnalysisDTO> AnalyzeVehicleTrendsAsync(int vehicleId);
    }
} 