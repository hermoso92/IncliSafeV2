using System.Threading.Tasks;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Analysis;
using IncliSafe.Shared.Models.DTOs;
using IncliSafe.Shared.Models.Entities;

namespace IncliSafe.Client.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<List<Vehiculo>> GetVehiculosAsync();
        Task<Vehiculo?> GetVehiculoAsync(int id);
        Task<Vehiculo> CreateVehiculoAsync(VehiculoDTO vehiculoDto);
        Task<bool> UpdateVehiculoAsync(int id, VehiculoDTO vehiculoDto);
        Task<bool> DeleteVehiculoAsync(int id);
        Task<List<Anomaly>> GetAnomaliesAsync(int vehicleId);
        Task<List<AnalysisPrediction>> GetPredictionsAsync(int vehicleId);
        Task<TrendAnalysis> GetTrendAnalysisAsync(int vehicleId);
    }
} 